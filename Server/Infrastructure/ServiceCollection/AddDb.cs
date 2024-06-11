using Microsoft.EntityFrameworkCore;
using Stl;
using Stl.Fusion.EntityFramework;
using Stl.Fusion.EntityFramework.Npgsql;
using Stl.Fusion.EntityFramework.Operations;
using Stl.Multitenancy;
using System.Data;

namespace Server.Infrastructure.ServiceCollection;

public static class AddDB
{
    public static IServiceCollection AddDataBase<TContext>(this IServiceCollection services, IWebHostEnvironment env,
        ConfigurationManager cfg, DataBaseType dataBaseType) where TContext : DbContext
    {
        services.AddTransient(_ => new DbOperationScope<TContext>.Options
        {
            DefaultIsolationLevel = IsolationLevel.RepeatableRead
        });

        services.AddDbContextServices<TContext>(ctx =>
        {
            ctx.AddOperations(operations =>
            {
                operations.ConfigureOperationLogReader(_ => new DbOperationLogReader<TContext>.Options
                {
                    UnconditionalCheckPeriod = TimeSpan.FromSeconds(env.IsDevelopment() ? 60 : 5)
                });
                if (dataBaseType == DataBaseType.PostgreSQL)
                {
                    operations.AddNpgsqlOperationLogChangeTracking();
                }
                else
                {
                    operations.AddFileBasedOperationLogChangeTracking();
                }
            });

            ctx.Services.AddDbContextFactory<TContext>((c, db) =>
            {
                var fakeTenant = new Tenant(default, "single", "single");
                switch (dataBaseType)
                {
                    case DataBaseType.PostgreSQL:
                        db.UseNpgsql(cfg!.GetConnectionString("Default")!.Interpolate(fakeTenant), x =>
                        {
                            x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            x.EnableRetryOnFailure(0);
                        });

                        db.UseNpgsqlHintFormatter();
                        break;
                }

                if (env.IsDevelopment()) db.EnableSensitiveDataLogging();
            });
        });

        return services;
    }
}

public enum DataBaseType
{
    PostgreSQL,
    SQLite
}