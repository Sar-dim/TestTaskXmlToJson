using System.Text.Json.Serialization;

namespace XmlRdfToJson.Data
{
    public class VoltageLevel
    {
        public string? Name { get; set; }
        
        [JsonIgnore]
        public string? Reference { get; set; }
        
        [JsonIgnore]
        public string? ReferenceToSubstation { get; set; }

        public List<SynchronousMachine> SynchronousMachines { get; set; } = new List<SynchronousMachine>();
    }
}
