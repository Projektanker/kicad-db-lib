using Splat;

namespace KiCadDbLib
{
    internal static class ReadonlyDependencyResolverExtensions
    {
        public static TService GetRequiredService<TService>(this IReadonlyDependencyResolver readonlyDependencyResolver)
        {
            return readonlyDependencyResolver.GetService<TService>()
                ?? throw new SimpleInjector.ActivationException($"Unable to activate {typeof(TService)}.");
        }
    }
}