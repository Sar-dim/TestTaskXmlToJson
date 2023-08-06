using XmlRdfToJson.Data;
using XmlRdfToJson.Extensions;

namespace XmlRdfToJson
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var substations = new List<Substation>();
            var voltageLevels = new List<VoltageLevel>();
            var synchronousMachines = new List<SynchronousMachine>();

            try
            {
                Services.FindAllSubstations(substations, Values.InputPath);
                Services.FindAllVoltageLevels(substations, voltageLevels, Values.InputPath);
                Services.FindAllSynchronousMachines(substations, voltageLevels, synchronousMachines, Values.InputPath);

                await Services.WriteSubstationsToFile(substations, Values.OutputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}