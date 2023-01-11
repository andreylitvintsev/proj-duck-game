using System;
using System.Linq;
using Leopotam.EcsLite;

namespace Logic.Extension
{
    public static class EcsWorldExtensions
    {
        public static ref T GetSingleton<T>(this EcsWorld world) where T : struct
        {
            var filter = world.Filter<T>().End();
            var count = filter.GetEntitiesCount();
            if (count > 1)
                throw new Exception($"Only one instance of component with type '{nameof(T)}' must be exists!");
            if (count == 0)
                return ref world.GetPool<T>().Add(world.NewEntity());
            return ref world.GetPool<T>().Get(filter.GetRawEntities().First());
        }
    }
}