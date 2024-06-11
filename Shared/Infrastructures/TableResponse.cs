using MemoryPack;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Shared.Infrastructures.Extensions
{
    [DataContract, MemoryPackable]
    public partial class TableResponse<T> where T : class
    {
        [property: DataMember] public List<T> Items { get; set; } = new List<T>();

        [property: DataMember] public int TotalItems { get; set; }
    }
}
