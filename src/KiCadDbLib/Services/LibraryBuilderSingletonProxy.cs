using System;
using System.Threading.Tasks;

namespace KiCadDbLib.Services
{
    public class LibraryBuilderSingletonProxy : ILibraryBuilder
    {
        private readonly Func<ILibraryBuilder> _factory;

        public LibraryBuilderSingletonProxy(Func<ILibraryBuilder> factory)
        {
            _factory = factory;
        }

        public Task Build()
        {
            var libraryBuilder = _factory();
            return libraryBuilder.Build();
        }
    }
}