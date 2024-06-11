using MemoryPack;
using Stl.Fusion;
using System.Runtime.Serialization;

namespace Shared.Features.Employees
{
    [DataContract, MemoryPackable]
    public partial record CreateEmployeeCommand([property: DataMember] Session Session, [property: DataMember] EmployeeView Entity) : ISessionCommand<EmployeeView>;

    [DataContract, MemoryPackable]
    public partial record UpdateEmployeeCommand([property: DataMember] Session Session, [property: DataMember] EmployeeView Entity) : ISessionCommand<EmployeeView>;

    [DataContract, MemoryPackable]
    public partial record DeleteEmployeeCommand([property: DataMember] Session Session, [property: DataMember] long Id) : ISessionCommand<EmployeeView>;
}
