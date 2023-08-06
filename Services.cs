using System.Xml.Linq;
using System.Xml;
using System.Text.Json;
using XmlRdfToJson.Data;
using System.Text.Encodings.Web;
using XmlRdfToJson.Extensions;

namespace XmlRdfToJson
{
    public static class Services
    {
        public static void FindAllSubstations(List<Substation> substations, string inputPath)
        {
            using (XmlReader reader = XmlReader.Create(inputPath))
            {
                while (!reader.EOF)
                {
                    if (reader.Name != Values.SubstationTag)
                    {
                        reader.ReadToFollowing(Values.SubstationTag);
                        if (!reader.EOF)
                        {
                            XElement itemSubstation = (XElement)XElement.ReadFrom(reader);

                            var reference = itemSubstation.Attributes()
                                .FirstOrDefault(x => x.Name.ToString()
                                .Contains(Values.AboutTag))?
                                .ToString().Split('\"')[1];
                            var name = itemSubstation.DescendantNodes()
                                .FirstOrDefault(x => x.ToString()
                                .Contains(Values.NameTag))?
                                .ToString().Split('>', '<')[2];

                            substations.Add(new Substation
                            {
                                Name = name,
                                Reference = reference
                            });
                        }
                    }
                }
            }
        }

        public static void FindAllVoltageLevels(List<Substation> substations, List<VoltageLevel> voltageLevels, string inputPath)
        {
            using (XmlReader reader = XmlReader.Create(inputPath))
            {
                while (!reader.EOF)
                {
                    if (reader.Name != Values.VoltageLevelTag)
                    {
                        reader.ReadToFollowing(Values.VoltageLevelTag);
                        if (!reader.EOF)
                        {
                            XElement itemVoltageLevel = (XElement)XElement.ReadFrom(reader);

                            var reference = itemVoltageLevel.Attributes()
                                .FirstOrDefault(x => x.Name.ToString()
                                .Contains(Values.AboutTag))?
                                .ToString().Split('\"')[1];
                            var name = itemVoltageLevel.DescendantNodes()
                                .FirstOrDefault(x => x.ToString()
                                .Contains(Values.NameTag))?
                                .ToString().Split('>', '<')[2];
                            var referenceToSubstation = itemVoltageLevel.DescendantNodes()
                                .FirstOrDefault(x => x.ToString()
                                .Contains(Values.VoltageLevelToSubstationTag))?
                                .ToString().Split('\"')[1];

                            var voltageLevel = new VoltageLevel
                            {
                                Name = name,
                                Reference = reference,
                                ReferenceToSubstation = referenceToSubstation
                            };

                            voltageLevels.Add(voltageLevel);

                            var substationOwner = substations.FirstOrDefault(x => x.Reference == voltageLevel.ReferenceToSubstation);
                            if (substationOwner != null)
                            {
                                substationOwner.VoltageLevels.Add(voltageLevel);
                            }
                        }
                    }
                }
            }
        }

        public static void FindAllSynchronousMachines(List<Substation> substations,
            List<VoltageLevel> voltageLevels, List<SynchronousMachine> synchronousMachines, string inputPath)
        {
            using (XmlReader reader = XmlReader.Create(inputPath))
            {
                while (!reader.EOF)
                {
                    if (reader.Name != Values.SynchronousMachineTag)
                    {
                        reader.ReadToFollowing(Values.SynchronousMachineTag);
                        if (!reader.EOF)
                        {
                            XElement itemSynchronousMachine = (XElement)XElement.ReadFrom(reader);

                            var reference = itemSynchronousMachine.Attributes()
                                .FirstOrDefault(x => x.Name.ToString()
                                .Contains(Values.AboutTag))?
                                .ToString().Split('\"')[1];
                            var name = itemSynchronousMachine.DescendantNodes()
                                .FirstOrDefault(x => x.ToString()
                                .Contains(Values.NameTag))?
                                .ToString().Split('>', '<')[2];
                            var referenceToVoltageLevel = itemSynchronousMachine.DescendantNodes()
                                .FirstOrDefault(x => x.ToString()
                                .Contains(Values.SynchronousMachineToVoltageLevelTag))?
                                .ToString().Split('\"')[1];

                            var synchronousMachine = new SynchronousMachine
                            {
                                Name = name,
                                Reference = reference,
                                ReferenceToVoltageLevel = referenceToVoltageLevel
                            };

                            synchronousMachines.Add(synchronousMachine);

                            var voltageLevelOwner = voltageLevels.FirstOrDefault(x => x.Reference == synchronousMachine.ReferenceToVoltageLevel);
                            if (voltageLevelOwner != null)
                            {
                                voltageLevelOwner.SynchronousMachines.Add(synchronousMachine);
                            }
                        }
                    }
                }
            }
        }
    
        public async static Task WriteSubstationsToFile(List<Substation> substations, string path)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(substations, options);

            using (StreamWriter writer = new StreamWriter(path, false))
            {
                await writer.WriteLineAsync(jsonString);
            }
        }
    }
}
