using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class AddPlayerPositionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerMarker>, Exc<Position>> _playerFilter;
        private readonly EcsPoolInject<Position> _positionPool;
        private readonly EcsCustomInject<IPlayerViewService> _playerViewService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                _positionPool.Value.Add(entity).Value = _playerViewService.Value.SpawnPosition.Value;
            }
        }
    }
}