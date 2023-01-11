using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class AddEnemyViewSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyMarker, InitialDirection>, Exc<EnemyView>> _enemyFilter;
        
        private readonly EcsPoolInject<EnemyView> _enemyViewPool;
        private readonly EcsPoolInject<InitialDirection> _initialDirectionPool;
        
        private readonly EcsCustomInject<IEnemyViewService> _enemyViewService;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _enemyFilter.Value)
            {
                _enemyViewService.Value.ApplyEntityCreation(entity);
                var view = _enemyViewService.Value.GetView(entity);
                _enemyViewPool.Value.Add(entity).Value = view;
                view.ApplyInitialDirection(_initialDirectionPool.Value.Get(entity));
            }
        }
    }
}