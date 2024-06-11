using MemoryPack;
using Stl.Fusion.Blazor;
using System.Runtime.Serialization;

namespace Shared.Features.Employees
{
    [DataContract, MemoryPackable]
    [ParameterComparer(typeof(ByValueParameterComparer))]
    public partial class EmployeeView
    {
        [property : DataMember] public long Id { get; set; }
        [property : DataMember] public string? PayrollNumber { get; set; }
        [property : DataMember] public string? Forenames { get; set; }
        [property : DataMember] public string? Surname { get; set; }
        [property : DataMember] public DateTime DateOfBirth { get; set; }
        [property : DataMember] public string? Telephone { get; set; }
        [property : DataMember] public string? Mobile { get; set; }
        [property : DataMember] public string? Address { get; set; }
        [property : DataMember] public string? Address2 { get; set; }
        [property : DataMember] public string? Postcode { get; set; }
        [property : DataMember] public string? EmailHome { get; set; }
        [property : DataMember] public DateTime StartDate { get; set; }
    }
}
}
