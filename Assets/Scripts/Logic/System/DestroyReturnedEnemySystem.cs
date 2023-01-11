using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Event;

namespace Logic.System
{
    public sealed class DestroyReturnedEnemySystem : IEcsRunSystem
    {
        private readonly EcsWorldInject _world;
        
        private readonly EcsFilterInject<Inc<EnemyReturnedToSpawnEvent>> _enemyFilter = "events";
        
        private readonly EcsPoolInject<EnemyReturnedToSpawnEvent> _enemyReturnedToSpawnEventPool = "events";
        private readonly EcsPoolInject<EnemyView> _enemyViewPool;
        private readonly EcsPoolInject<MinigameView> _minigameViewPool;

        private readonly EcsCustomInject<IEnemyViewService> _enemyViewService;
        private readonly EcsCustomInject<IMinigameViewService> _minigameViewService;

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _enemyFilter.Value)
            {
                var enemy = _enemyReturnedToSpawnEventPool.Value.Get(eventEntity).Enemy;
                if (_enemyViewPool.Value.Has(enemy))
                    _enemyViewService.Value.ApplyEntityDestruction(enemy);
                if (_minigameViewPool.Value.Has(enemy))
                    _minigameViewService.Value.ApplyEntityDestruction(enemy);
                _world.Value.DelEntity(enemy);
            }
        }
    }
}