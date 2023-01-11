using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;

namespace Logic.System
{
    public sealed class DeleteViewsOnDestroySystem : IEcsDestroySystem
    {
        private readonly EcsFilterInject<Inc<EnemyView>> _enemyViewFilter;
        private readonly EcsFilterInject<Inc<MinigameView>> _minigameViewFilter;
        
        private readonly EcsPoolInject<EnemyView> _enemyViewPool;
        private readonly EcsPoolInject<MinigameView> _minigameViewPool;

        private readonly EcsCustomInject<IEnemyViewService> _enemyViewService;
        private readonly EcsCustomInject<IMinigameViewService> _minigameViewService;

        public void Destroy(IEcsSystems systems)
        {
            foreach (var enemyEntity in _enemyViewFilter.Value)
            {
                _enemyViewService.Value.ApplyEntityDestruction(enemyEntity);
            }

            foreach (var minigameEntity in _minigameViewFilter.Value)
            {
                _minigameViewService.Value.ApplyEntityDestruction(minigameEntity);
            }
        }
    }
}