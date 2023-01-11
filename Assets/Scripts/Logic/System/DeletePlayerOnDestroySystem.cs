using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class DeletePlayerOnDestroySystem : IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<PlayerMarker>> _playerFilter;
        private readonly EcsCustomInject<IPlayerViewService> _playerViewService;
        private readonly EcsPoolInject<PlayerView> _playerViewPool;
        private readonly EcsWorldInject _world;

        public void Destroy(IEcsSystems systems)
        {
            foreach (var entity in _playerFilter.Value)
            {
                if (_playerViewPool.Value.Has(entity))
                    _playerViewService.Value.ApplyEntityDestruction(entity);
                _world.Value.DelEntity(entity);
            }
        }
    }
}