using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Logic.Extension
{
    public struct EcsSingletonFilterInject<T> : IEcsDataInject
        where T : struct
    {
        private string _worldName;
        private EcsWorld _world;
        private EcsPool<T> _pool;
        private EcsFilter _filter;
        
        public static implicit operator EcsSingletonFilterInject<T> (string worldName) {
            return new EcsSingletonFilterInject<T> { _worldName = worldName };
        }

        void IEcsDataInject.Fill(IEcsSystems systems)
        {
            _world = systems.GetWorld(_worldName);
            _pool = _world.GetPool<T>();
            _filter = _world.Filter<T>().End();
        }

        public ref T Value => ref _pool.Get(Entity);
        public int Entity
        {
            get
            {
                if (_filter.GetEntitiesCount() > 1)
                    throw new Exception($"Only one instance of component with type '{nameof(T)}' must be exists!");

                if (_filter.GetEntitiesCount() == 0)
                {
                    _pool.Add(_world.NewEntity());
                }

                using (var enumerator = _filter.GetEnumerator())
                {
                    enumerator.MoveNext();
                    return enumerator.Current;
                }
            }
        } 
    }
}