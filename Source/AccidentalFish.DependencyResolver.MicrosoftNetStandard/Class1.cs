using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace AccidentalFish.DependencyResolver.MicrosoftNetStandard
{
    public class MicrosoftNetStandardDependencyResolver : IDependencyResolver
    {
        private readonly IServiceCollection _serviceCollection;
        private readonly HashSet<Type> _registeredTypes = new HashSet<Type>();
        private IServiceProvider _serviceProvider = null;

        public MicrosoftNetStandardDependencyResolver(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public IServiceProvider BuildServiceProvider()
        {
            _serviceProvider = _serviceCollection.BuildServiceProvider();
            return _serviceProvider;
        }

        public IServiceProvider SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return _serviceProvider;
        }

        public IDependencyResolver Register<T1, T2>() where T2 : T1
        {
            _serviceCollection.AddTransient(typeof(T1), typeof(T2));
            _registeredTypes.Add(typeof(T1));
            return this;
        }

        public IDependencyResolver Register<T1>(Func<T1> creator)
        {
            _serviceCollection.AddTransient(typeof(T1), (sp) => creator());
            _registeredTypes.Add(typeof(T1));
            return this;
        }

        public IDependencyResolver Register<T1, T2>(string name) where T2 : T1
        {
            throw new NotImplementedException("Named dependencies are not supported with IServiceCollection");
        }

        public IDependencyResolver Register(Type type1, Type type2)
        {
            _serviceCollection.AddTransient(type1, type2);
            _registeredTypes.Add(type1);
            return this;
        }

        public IDependencyResolver RegisterInstance<T>(T instance)
        {
            _serviceCollection.AddSingleton(typeof(T), instance);
            return this;
        }

        public bool IsRegistered<T>()
        {
            return _registeredTypes.Contains(typeof(T));
        }

        public T Resolve<T>()
        {
            return _serviceProvider.GetService<T>();
        }

        public T Resolve<T>(string name)
        {
            throw new NotImplementedException("Named dependencies are not supported with IServiceCollection");
        }

        public object Resolve(Type type)
        {
            return _serviceProvider.GetService(type);
        }

        public object Resolve(Type type, string name)
        {
            throw new NotImplementedException("Named dependencies are not supported with IServiceCollection");
        }
    }
}
