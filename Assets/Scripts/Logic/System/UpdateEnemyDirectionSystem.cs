using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class UpdateEnemyDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyMarker, EnemyState, InitialDirection, Direction>> _enemyFilter;
        
        private readonly EcsPoolInject<EnemyState> _enemyStatePool;
        private readonly EcsPoolInject<InitialDirection> _initialDirectionPool;
        private readonly EcsPoolInject<Direction> _directionPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var enemyEntity in _enemyFilter.Value)
            {
                if (_enemyStatePool.Value.Get(enemyEntity).Value == EnemyStateValue.Returning)
                    _directionPool.Value.Get(enemyEntity).IsLeft = !_initialDirectionPool.Value.Get(enemyEntity).IsLeft;
            }
        }
    }
}