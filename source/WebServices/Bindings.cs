using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebServices.DAO;
using MongoDB.Driver;
using System.Configuration;

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
        }
    }
}