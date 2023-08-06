using System.Text.Json.Serialization;

namespace XmlRdfToJson.Data
{
    public class SynchronousMachine
    {
        public string? Name { get; set; }
        
        [JsonIgnore]
        public string? Reference { get; set; }
        
        [JsonIgnore]
        public string? ReferenceToVoltageLevel { get; set; }
    }
}
