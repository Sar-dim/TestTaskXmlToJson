using System.Text.Json.Serialization;

namespace XmlRdfToJson.Data
{
    public class Substation
    {
        public string? Name { get; set; }

        [JsonIgnore]
        public string? Reference { get; set; }
        
        public List<VoltageLevel> VoltageLevels { get; set; } = new List<VoltageLevel>();
    }
}
