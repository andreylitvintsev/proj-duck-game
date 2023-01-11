using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    // TODO: прибраться
    public sealed class AddOrRemoveNearestToPlayerMarkerSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyView, EnemyState, InitialDirection>> _positionFilter;
        
        private readonly EcsPoolInject<EnemyView> _enemyViewPool;
        private readonly EcsPoolInject<EnemyState> _enemyStatePool;
        private readonly EcsPoolInject<InitialDirection> _initialDirectionPool;
        private readonly EcsPoolInject<NearestEnemyMarker> _nearestEnemyMarkerPool;

        private readonly EcsCustomInject<IEnemyViewService> _enemyViewService;

        public void Run(IEcsSystems systems)
        {
            int? nearestLeftEnemy = null;
            int? nearestRightEnemy = null;

            bool nearestLeftEnemyAttacking = false;
            bool nearestRightEnemyAttacking = false;
            
            foreach (var entity in _positionFilter.Value)
            {
                _nearestEnemyMarkerPool.Value.Del(entity);

                var enemyView = _enemyViewPool.Value.Get(entity).Value;
                var enemyState = _enemyStatePool.Value.Get(entity);
                var initialDirection = _initialDirectionPool.Value.Get(entity);
                var position = enemyView.Position;
                var targetPosition = enemyView.PlayerPosition;
                
                if (enemyState.Value is EnemyStateValue.Attacking or EnemyStateValue.AngryWalking)
                {
                    if (initialDirection.IsLeft)
                        nearestRightEnemyAttacking = true;
                    else
                        nearestLeftEnemyAttacking = true;
                }
                else if (enemyState.Value == EnemyStateValue.Walking)
                {
                    if (initialDirection.IsLeft)
                    {
                        if (nearestRightEnemy == null)
                        {
                            nearestRightEnemy = entity;
                            continue;
                        }

                        var otherPosition = _enemyViewPool.Value.Get(nearestRightEnemy.Value).Value.Position;
                        if ((targetPosition - position).Length() < (targetPosition - otherPosition).Length()) // TODO: изменить на математику векторов
                            nearestRightEnemy = entity;
                    }
                    else // isRight
                    {
                        if (nearestLeftEnemy == null)
                        {
                            nearestLeftEnemy = entity;
                            continue;
                        }
                        
                        var otherPosition = _enemyViewPool.Value.Get(nearestLeftEnemy.Value).Value.Position;
                        if ((targetPosition - position).Length() < (targetPosition - otherPosition).Length()) // TODO: изменить на математику векторов
                            nearestLeftEnemy = entity;
                    }
                }
            }

            if (nearestLeftEnemy != null && !nearestLeftEnemyAttacking)
                _nearestEnemyMarkerPool.Value.Add(nearestLeftEnemy.Value);
            
            if (nearestRightEnemy != null && !nearestRightEnemyAttacking)
                _nearestEnemyMarkerPool.Value.Add(nearestRightEnemy.Value);
        }
    }
}