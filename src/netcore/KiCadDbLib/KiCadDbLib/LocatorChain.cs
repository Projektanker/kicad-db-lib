using System;
using System.Collections.Generic;
using System.Linq;
using SimpleInjector;
using Splat;

namespace KiCadDbLib
{
    internal sealed class LocatorChain : IDependencyResolver
    {
        private readonly Container _container;
        private readonly IDependencyResolver _dependencyResolver;

        public LocatorChain(IDependencyResolver dependencyResolver, Container container)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void Dispose()
        {
            _dependencyResolver.Dispose();
            _container.Dispose();
        }

        public object GetService(Type serviceType, string contract = null)
        {
            return _dependencyResolver.GetService(serviceType, contract)
                ?? ((IServiceProvider)_container).GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            var services = _dependencyResolver.GetServices(serviceType, contract);
            if (ContainerHasRegistration(serviceType))
            {
                services.Concat(_container.GetAllInstances(serviceType));
            }

            return services;
        }

        public bool HasRegistration(Type serviceType, string contract = null)
        {
            return _dependencyResolver.HasRegistration(serviceType, contract)
                || ContainerHasRegistration(serviceType);
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            _dependencyResolver.Register(factory, serviceType, contract);
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            return _dependencyResolver.ServiceRegistrationCallback(serviceType, contract, callback);
        }

        public void UnregisterAll(Type serviceType, string contract = null)
        {
            _dependencyResolver.UnregisterAll(serviceType, contract);
        }

        public void UnregisterCurrent(Type serviceType, string contract = null)
        {
            _dependencyResolver.UnregisterCurrent(serviceType, contract);
        }

        private bool ContainerHasRegistration(Type serviceType)
        {
            return _container.GetCurrentRegistrations().Any(x => x.ServiceType == serviceType);
        }
    }
}