using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace WebServices.Algorithms
{
    public interface IGraphComponents
    {
        Task<string[][]> GetGraphComponents();
    }
}