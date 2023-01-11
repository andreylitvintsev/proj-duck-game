using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class AddOrDestroyMinigameViewSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<NearestEnemyMarker>, Exc<MinigameView>> _enemyFilter;
        private readonly EcsFilterInject<Inc<MinigameView>, Exc<NearestEnemyMarker>> _minigameFilter;

        private readonly EcsPoolInject<MinigameView> _minigameViewPool;

        private readonly EcsCustomInject<IMinigameViewService> _minigameViewService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _minigameFilter.Value)
            {
                _minigameViewService.Value.ApplyEntityDestruction(entity);
                _minigameViewPool.Value.Del(entity);
            }

            foreach (var entity in _enemyFilter.Value)
            {
                _minigameViewService.Value.ApplyEntityCreation(entity);
                _minigameViewPool.Value.Add(entity).Value = _minigameViewService.Value.GetView(entity);
            }
        }
    }
}