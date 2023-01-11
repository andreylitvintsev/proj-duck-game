using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;
using Logic.Extension;

namespace Logic.System
{
    public sealed class AddOrUpdatePlayerDirectionSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<PlayerMarker>> _directionFilter;
        private readonly EcsPoolInject<Direction> _directionPoolInject;
        private readonly EcsWorldInject _world;

        private readonly EcsCustomInject<IInputService> _inputService;
        
        public void Run(IEcsSystems systems)
        {
            var isPressedToLeft = _inputService.Value.PressedToLeft;
            var isPressedToRight = _inputService.Value.PressedToRight;
            if (isPressedToLeft == isPressedToRight)
                return;
            
            foreach (var entity in _directionFilter.Value)
            {
                _directionPoolInject.Value.GetOrAdd(entity).IsLeft = isPressedToLeft;
            }
        }
    }
}