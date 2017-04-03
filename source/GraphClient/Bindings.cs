using System;
using Ninject.Modules;
using System.Configuration;
using GraphClient.DataProvider;

namespace GraphClient
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IGraphDataProvider>().To<WebServiceProvider>();
            //Bind<IGraphDataProvider>().To<TestGraphDataProvider>();
            Bind<GraphDataService.GraphDataServiceClient>().ToSelf();
            Bind<GraphQueryService.GraphQueryServiceClient>().ToSelf();
        }
    }
}