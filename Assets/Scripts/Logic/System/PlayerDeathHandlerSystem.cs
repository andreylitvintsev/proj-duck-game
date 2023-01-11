using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Extension;

namespace Logic.System
{
    public sealed class PlayerDeathHandlerSystem : IEcsRunSystem
    {
        private readonly EcsSingletonFilterInject<PlayerHealth> _playerHealth;

        private readonly EcsCustomInject<IPlayerDeathHandlerService> _playerDeathService;

        public void Run(IEcsSystems systems)
        {
            if (_playerHealth.Value.Value <= 0)
                _playerDeathService.Value.HandleDeath();
        }
    }
}