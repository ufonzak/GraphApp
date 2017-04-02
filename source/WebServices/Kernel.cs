using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using System.Reflection;

namespace WebServices
{
    public static class Kernel
    {
        public static StandardKernel Instance { get; private set; }

        static Kernel()
        {
            Instance = new StandardKernel();
            Instance.Load(Assembly.GetExecutingAssembly());
        }

        public static T Get<T>()
        {
            return Instance.Get<T>();
        }
    }
}