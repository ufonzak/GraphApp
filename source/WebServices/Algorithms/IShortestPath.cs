using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebServices.Algorithms
{
    public interface IShortestPath
    {
        Task<string[]> GetShortestPath(string nodeIdFrom, string nodeIdTo);
    }

    public class ShortestPathException : Exception
    {
        public ShortestPathException(string message) : base (message)
        {

        }
    }
}