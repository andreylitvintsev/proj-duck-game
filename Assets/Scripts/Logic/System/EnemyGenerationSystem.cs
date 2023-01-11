using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component;
using Logic.Component.Marker;

namespace Logic.System
{
    public sealed class EnemyGenerationSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsPoolInject<EnemyMarker> _enemyMarkerPool;
        private readonly EcsPoolInject<EnemyVariant> _enemyVariantPool;
        private readonly EcsPoolInject<InitialDirection> _initialDirectionPool;
        private readonly EcsPoolInject<Direction> _directionPool;
        private readonly EcsPoolInject<Position> _positionPool;
        private readonly EcsPoolInject<EnemyState> _enemyStatePool;
        private readonly EcsPoolInject<MinigameMarker> _minigameMarkerPool;

        private readonly EcsWorldInject _world;
        
        private readonly EcsCustomInject<ITimeService> _timeService;
        private readonly EcsCustomInject<IEnemyViewService> _enemyViewService;
        private readonly EcsCustomInject<IMinigameViewService> _minigameViewService;
        private readonly EcsCustomInject<IRandomService> _randomService;

        private int _totalGeneratedEnemiesCount;
        private float _backwardTimer;

        public void Init(IEcsSystems systems)
        {
            SetupTimer();
        }

        public void Run(IEcsSystems systems)
        {
            _backwardTimer -= _timeService.Value.DeltaTime;
            if (_backwardTimer <= 0)
            {
                ++_totalGeneratedEnemiesCount;
                GenerateEnemy();
                SetupTimer();
            }
        }

        private void GenerateEnemy()
        {
            var entity = _world.Value.NewEntity();
            //
            _enemyMarkerPool.Value.Add(entity);
            // todo
            _enemyVariantPool.Value.Add(entity).Value =
                _randomService.Value.Boolean() ? EnemyVariantValue.Girl : EnemyVariantValue.Guy;
            //
            _directionPool.Value.Add(entity).IsLeft = 
                _initialDirectionPool.Value.Add(entity).IsLeft = _randomService.Value.Boolean();
            //
            _enemyStatePool.Value.Add(entity).Value = EnemyStateValue.Walking;
            //
            _minigameMarkerPool.Value.Add(entity);
        }

        private void SetupTimer()
        {
            _backwardTimer = _enemyViewService.Value.GetDelayToGenerate(_totalGeneratedEnemiesCount);
        }
    }
}