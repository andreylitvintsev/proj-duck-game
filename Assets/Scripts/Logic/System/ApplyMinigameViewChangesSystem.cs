using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;

namespace Logic.System
{
    public sealed class ApplyMinigameViewChangesSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<MinigameView>> _minigameFilter;

        private readonly EcsPoolInject<EnemyView> _enemyViewPool;

        private readonly EcsCustomInject<ITimeService> _timeService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _minigameFilter.Value)
            {
                var view = _minigameFilter.Pools.Inc1.Get(entity).Value;
                if (_enemyViewPool.Value.Has(entity))
                    view.ApplyPosition(_enemyViewPool.Value.Get(entity).Value.Position);
                view.ApplyUpdate(_timeService.Value);
            }
        }
    }
}