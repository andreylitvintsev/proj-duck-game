using Leopotam.EcsLite;

namespace Logic.Extension
{
    public static class EcsPoolExtensions
    {
        public static ref T GetOrAdd<T>(this EcsPool<T> pool, int entity) where T : struct
        {
            if (!pool.Has(entity))
                pool.Add(entity);
            return ref pool.Get(entity);
        }
    }
}