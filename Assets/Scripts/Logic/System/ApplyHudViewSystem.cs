using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Extension;

namespace Logic.System
{
    public sealed class ApplyHudViewSystem : IEcsRunSystem
    {
        private readonly EcsSingletonFilterInject<PlayerHealth> _playerHealth;
        private readonly EcsSingletonFilterInject<SuccessfulAttackCount> _attackedEnemiesCount;
        private readonly EcsCustomInject<IHudViewService> _hudViewService;

        public void Run(IEcsSystems systems)
        {
            _hudViewService.Value.ApplyPlayerHealth(_playerHealth.Value);
            _hudViewService.Value.ApplyAttackedEnemiesCount(_attackedEnemiesCount.Value);
        }
    }
}