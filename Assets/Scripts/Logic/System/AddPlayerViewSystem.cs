using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class AddPlayerViewSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PlayerMarker>, Exc<PlayerView>> _playerFilter;
        private EcsPoolInject<PlayerView> _playerViewPool;
        private EcsCustomInject<IPlayerViewService> _playerViewService;

        public void Run(IEcsSystems systems)
        {
            foreach (var enitiy in _playerFilter.Value)
            {
                _playerViewService.Value.ApplyEntityCreation(enitiy);
                _playerViewPool.Value.Add(enitiy).Value = _playerViewService.Value.GetView(enitiy);
            }
        }
    }
}