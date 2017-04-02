using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServices.DAO;

namespace WebServices
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IGraphNodeDAO>().To<GraphNodeDAO>().InSingletonScope();
        }
    }
}