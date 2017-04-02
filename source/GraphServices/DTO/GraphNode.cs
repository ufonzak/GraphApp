using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace GraphServices.DTO
{
    [DataContract]
    public class GraphNode
    {
        [DataMember]
        public string ID { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public List<string> AdjacentNodeIDs { get; set; }
    }
}
