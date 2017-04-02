using System;
using Ninject;
using System.Reflection;

namespace GraphClient
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