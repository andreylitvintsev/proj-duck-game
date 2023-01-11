using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;

namespace Logic.System
{
    public sealed class ApplyEnemyViewChangesSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyView>> _ecsFilter;
        private readonly EcsPoolInject<EnemyVariant> _enemyVariantPool;
        private readonly EcsPoolInject<Direction> _directionPool;
        private readonly EcsPoolInject<EnemyState> _enemyStatePool;

        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _ecsFilter.Value)
            {
                var view = _ecsFilter.Pools.Inc1.Get(entity).Value;
                if (_enemyVariantPool.Value.Has(entity)) 
                    view.ApplyEnemyVariant(_enemyVariantPool.Value.Get(entity));
                if (_directionPool.Value.Has(entity))
                    view.ApplyDirection(_directionPool.Value.Get(entity));
                if (_enemyStatePool.Value.Has(entity))
                    view.ApplyEnemyState(_enemyStatePool.Value.Get(entity));
                view.ApplyUpdate(_timeService.Value);
            }
        }
    }
}