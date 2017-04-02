using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using GraphServices.DTO;

namespace WebServices.DAO
{
    public class GraphNodeModel
    {
        [BsonId]
        public string ID { get; set; }

        public string Label { get; set; }

        public List<string> AdjacentNodeIDs { get; set; }

        public bool IsValid { get; set; }

        public GraphNode ToDTO()
        {
            return new GraphNode()
            {
                ID = ID,
                Label = Label,
                AdjacentNodeIDs = AdjacentNodeIDs != null ? AdjacentNodeIDs.ToList() : new List<string>()
            };
        }
    }
}