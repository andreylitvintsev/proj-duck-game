using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;

namespace Logic.System
{
    public sealed class ApplyPlayerViewChangesSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerView>> _filter;
        private readonly EcsPoolInject<Position> _positionPool;
        private readonly EcsPoolInject<Direction> _directionPool;
        private readonly EcsPoolInject<IsAttacking> _isAttackingPool;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var view = _filter.Pools.Inc1.Get(entity).Value;
                if (_positionPool.Value.Has(entity))
                    view.ApplyPosition(_positionPool.Value.Get(entity));
                if (_directionPool.Value.Has(entity))
                    view.ApplyDirection(_directionPool.Value.Get(entity));
                if (_isAttackingPool.Value.Has(entity))
                    view.ApplyIsAttacking(_isAttackingPool.Value.Get(entity));
            }
        }
    }
}