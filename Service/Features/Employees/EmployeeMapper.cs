using Riok.Mapperly.Abstractions;
using Shared.Features.Employees;

namespace Service.Features.Employees
{
    [Mapper]
    public static partial class EmployeeMapper
    {
        #region Usable
        public static EmployeeView MapToView(this EmployeeEntity src) => src.To();
        public static List<EmployeeView> MapToViewList(this List<EmployeeEntity> src) => src.ToList();
        public static EmployeeEntity MapFromView(this EmployeeView src) => src.From();
        #endregion

        #region Internal

        private static partial EmployeeView To(this EmployeeEntity src);

        private static partial List<EmployeeView> ToList(this List<EmployeeEntity> src);

        private static partial EmployeeEntity From(this EmployeeView src);

        public static partial void From(EmployeeView View, EmployeeEntity Entity);

        #endregion
    }
}
