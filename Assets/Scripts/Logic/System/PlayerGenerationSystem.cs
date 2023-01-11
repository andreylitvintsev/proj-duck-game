using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;
using Logic.Extension;

namespace Logic.System
{
    public sealed class PlayerGenerationSystem : IEcsInitSystem
    {
        private readonly EcsSingletonFilterInject<PlayerMarker> _playerMarker;
        private readonly EcsPoolInject<PlayerHealth> _healthPool;
        private readonly EcsPoolInject<SuccessfulAttackCount> _attackedEnemiesCountPool;
        private readonly EcsWorldInject _world;
        private readonly EcsCustomInject<IPlayerViewService> _playerViewService;

        public void Init(IEcsSystems systems)
        {
            _healthPool.Value.Add(_playerMarker.Entity).Value = _playerViewService.Value.InitialHealthValue;
            _attackedEnemiesCountPool.Value.Add(_playerMarker.Entity);
        }
    }
}