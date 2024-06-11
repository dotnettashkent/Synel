using Service.Data;
using Stl.Fusion;
using Stl.Fusion.Authentication;
using Stl.Fusion.Blazor;
using Stl.Fusion.Blazor.Authentication;
using Stl.Fusion.Extensions;
using Stl.Fusion.Server;
using Stl.Rpc;

namespace Server.Infrastructure.ServiceCollection
{
    public static class FusionServices
    {
        public static IServiceCollection AddFusionServices(this IServiceCollection services)
        {
            // Fusion services
            var fusion = services.AddFusion(RpcServiceMode.Server, true);

            var fusionServer = fusion.AddWebServer();


            fusionServer.ConfigureAuthEndpoint(_ => new()
            {
                DefaultScheme = "oidc",
                SignInPropertiesBuilder = (_, properties) =>
                {
                    properties.IsPersistent = true;
                }
            });
            fusionServer.ConfigureServerAuthHelper(_ => new()
            {
                NameClaimKeys = Array.Empty<string>(),
            });

            fusion.AddSandboxedKeyValueStore();
            fusion.AddOperationReprocessor();

            fusion.AddBlazor().AddAuthentication().AddPresenceReporter();

            fusion.AddDbAuthService<AppDbContext, string>();
            fusion.AddDbKeyValueStore<AppDbContext>();
            fusion.AddEmployeeServices();

            return services;
        }
    }
}
