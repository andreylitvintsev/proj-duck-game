using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;
using Logic.Extension;

namespace Logic.System
{
    public sealed class AddOrUpdatePlayerAttackSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerMarker>> _playerFilter;
        private readonly EcsPoolInject<IsAttacking> _isAttackingPool;
        private readonly EcsWorldInject _world;

        private readonly EcsCustomInject<IInputService> _inputService;

        public void Run(IEcsSystems systems)
        {
            var isPressedToLeft = _inputService.Value.PressedToLeft;
            var isPressedToRight = _inputService.Value.PressedToRight;
            var isPressed = isPressedToLeft || isPressedToRight;
            
            foreach (var entity in _playerFilter.Value)
            {
                _isAttackingPool.Value.GetOrAdd(entity).Value = isPressed;
            }
        }
    }
}