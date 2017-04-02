using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace WebServices.DAO
{
    [DataContract]
    public class GraphNode
    {
        [DataMember]
        [BsonId]
        public string ID { get; set; }

        [DataMember]
        public string Label { get; set; }

        [DataMember]
        public List<string> AdjacentNodeIDs { get; set; }

        public bool IsValid { get; set; }
    }
}