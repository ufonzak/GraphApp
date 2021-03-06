﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphServices.DTO;

namespace GraphClient.DataProvider
{
    interface IGraphDataProvider
    {
        Task<GraphNode[]> GetAllNodes();

        Task<string[]> GetShortestPath(string nodeFrom, string nodeTo);
        Task<string[][]> GetGraphComponents();
    }
}
