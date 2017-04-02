using System;
using Ninject.Modules;
using System.Configuration;
using WebServices.DAO;
using MongoDB.Driver;

namespace WebServices
{
    public class Bindings : NinjectModule
    {
        public override void Load()
        {
            Bind<IGraphNodeDAO>().To<GraphNodeDAO>().InSingletonScope();
            Bind<IMongoClient>().ToMethod(p => new MongoClient(ConfigurationManager.AppSettings["mongodbConnectionString"])).InSingletonScope();
            Bind<IMongoDatabase>().ToMethod(p =>
            {
                IMongoClient client = p.Kernel.GetService(typeof(IMongoClient)) as IMongoClient;
                return client.GetDatabase(ConfigurationManager.AppSettings["mongodbDatabaseName"]);
            }).InSingletonScope();
            Bind<Algorithms.IShortestPath>().To<Algorithms.BfsShortestPath>();
            Bind<Algorithms.IGraphComponents>().To<Algorithms.DfsGraphComponents>();
        }
    }
}