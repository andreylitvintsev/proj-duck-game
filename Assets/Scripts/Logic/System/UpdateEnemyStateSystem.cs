using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Event;
using Logic.Component.Marker;
using Logic.Extension;

namespace Logic.System
{
    public sealed class UpdateEnemyStateSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EnemyMovedToPlayerEvent>> _movedToPlayerEventFilter = "events";
        private readonly EcsFilterInject<Inc<EnemyAttackFinishedEvent>> _attackFinishedEventFilter = "events";
        private readonly EcsFilterInject<Inc<NearestEnemyMarker, EnemyState, MinigameView>> _nearestEnemyFilter;

        private readonly EcsSingletonFilterInject<SuccessfulAttackCount> _successfulAttackCount;
        private readonly EcsSingletonFilterInject<PlayerHealth> _playerHealth;

        private readonly EcsPoolInject<EnemyMovedToPlayerEvent> _enemyMovedToPlayerEventPool = "events";
        private readonly EcsPoolInject<EnemyAttackFinishedEvent> _enemyAttackFinishedEventPool = "events";
        private readonly EcsPoolInject<EnemyState> _enemyStatePool;
        private readonly EcsPoolInject<InitialDirection> _initialDirectionPool;
        private readonly EcsPoolInject<Direction> _directionPool;
        private readonly EcsPoolInject<MinigameView> _minigameViewPool;

        private readonly EcsCustomInject<IInputService> _inputService;

        public void Run(IEcsSystems systems) // TODO: разбить на методы
        {
            var isClicked = _inputService.Value.ClickedToLeft || _inputService.Value.ClickedToRight;
            var isClickedToLeft = _inputService.Value.ClickedToLeft;
            if (isClicked)
            {
                foreach (var enemyEntity in _nearestEnemyFilter.Value)
                {
                    var initialDirection = _initialDirectionPool.Value.Get(enemyEntity);
                    ref var enemyState = ref _enemyStatePool.Value.Get(enemyEntity);
                    var canWin = _minigameViewPool.Value.Get(enemyEntity).Value.CanWin();
                    if (enemyState.Value == EnemyStateValue.Walking && initialDirection.IsLeft != isClickedToLeft)
                        if (canWin)
                        {
                            ++_successfulAttackCount.Value.Value;
                            enemyState.Value = EnemyStateValue.Returning;
                        }
                        else
                        {
                            enemyState.Value = EnemyStateValue.AngryWalking;
                        }
                }
            }

            foreach (var eventEntity in _movedToPlayerEventFilter.Value)
            {
                var enemy = _enemyMovedToPlayerEventPool.Value.Get(eventEntity).Enemy;
                if (_enemyStatePool.Value.Has(enemy))
                    _enemyStatePool.Value.Get(enemy).Value = EnemyStateValue.Attacking;
            }

            foreach (var eventEntity in _attackFinishedEventFilter.Value)
            {
                var enemy = _enemyAttackFinishedEventPool.Value.Get(eventEntity).Enemy;
                if (_enemyStatePool.Value.Has(enemy))
                {
                    --_playerHealth.Value.Value;
                    _enemyStatePool.Value.Get(enemy).Value = EnemyStateValue.Returning;
                }
            }
        }
    }
}