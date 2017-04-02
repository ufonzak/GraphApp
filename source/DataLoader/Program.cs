using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace DataLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: DataLoader.exe [import directory]");
                Environment.Exit(1);
            }
            string directoryPath = args[0];
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine("Directory does not exist.");
                Environment.Exit(1);
            }

            var serializer = new XmlSerializer(typeof(GraphNodeData));

            try
            {
                Services.GraphManagementServiceClient client = new Services.GraphManagementServiceClient();

                Console.WriteLine("Invalidating graph nodes.");
                client.InvalidateAllGraphNodes();

                DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
                foreach (FileInfo file in directoryInfo.GetFiles("*.xml"))
                {
                    using (TextReader reader = new StreamReader(file.FullName))
                    {
                        GraphNodeData node = serializer.Deserialize(reader) as GraphNodeData;
                        client.SyncGraphNode(new Services.GraphNode()
                        {
                            ID = node.ID,
                            Label = node.Label,
                            AdjacentNodeIDs = node.AdjacentNodes
                        });
                        Console.WriteLine($"Node {node.ID} imported.");
                    }
                }

                Console.WriteLine("Deleting invalid nodes.");
                client.DeleteAllInvalidGraphNodes();

                Console.WriteLine("Normalizing relations.");
                client.NormalizeRelations();

                client.Close();

                Console.WriteLine("Synchronization sucessfuly completed");
            }
            catch (Exception er)
            {
                Console.WriteLine("There was an error during import:");
                Console.WriteLine(er);

                Environment.Exit(1);
            }
        }
    }

    [Serializable]
    [XmlRoot("node")]
    public class GraphNodeData
    {
        [XmlElement("id")]
        public string ID { get; set; }

        [XmlElement("label")]
        public string Label { get; set; }

        [XmlArray("adjacentNodes")]
        [XmlArrayItem(ElementName = "id")]
        public string[] AdjacentNodes { get; set; }
    }
}
