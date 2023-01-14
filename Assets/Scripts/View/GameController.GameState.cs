using System.Collections;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Logic.Component.Event;
using Logic.Extension.EcsGenerateEventsSystem;
using Logic.System;
using StateMachine;
using UnityEngine;

namespace View
{
    public sealed partial class GameController
    {
        private sealed class GameState : IState<GameController>
        {
            private EcsWorld _world;
            private EcsWorld _eventsWorld;
            private IEcsSystems _gameplayEcsSystems;

            private TimeService _timeService;
            private InputService _inputService;
            private PlayerViewService _playerViewService;
            private EnemyViewService _enemyViewService;
            private MinigameViewService _minigameViewService;
            private RandomService _randomService;
            private HudViewService _hudViewService;

            private readonly PlayerDeathHandlerService _playerDeathHandlerService;

            public GameState()
            {
                _playerDeathHandlerService = new PlayerDeathHandlerService();
            }

            public IEnumerator OnEnterState(GameController context)
            {
                _timeService = new TimeService();
                _inputService = new InputService();
                _playerViewService = new PlayerViewService(
                    playerViewPrefab: context._playerViewPrefab,
                    spawnPosition: context._playerSpawnPoint.position,
                    initialHealthValue: context._initialPlayerHealthValue
                );
                _enemyViewService = new EnemyViewService(
                    enemyViewPrefab: context._enemyViewPrefab,
                    viewConfig: context,
                    eventCollectorHolder: context,
                    delayToGenerateChart: context._delayToGenerateEnemyChart
                );
                _minigameViewService = new MinigameViewService(
                    canvas: context._minigamesCanvas,
                    minigameViewPrefab: context._minigameViewPrefab,
                    viewConfig: context
                );
                _randomService = new RandomService();
                _hudViewService = new HudViewService(context._initialPlayerHealthValue);

                _world = new EcsWorld();
                _eventsWorld = new EcsWorld();

                _gameplayEcsSystems = new EcsSystems(_world)
                    .AddWorld(_eventsWorld, "events")
                    .Add(new EcsGenerateEventsSystem<EnemyMovedToPlayerEvent>(_eventsWorld,
                        context._nonEcsEventCollector))
                    .Add(new EcsGenerateEventsSystem<EnemyAttackFinishedEvent>(_eventsWorld,
                        context._nonEcsEventCollector))
                    .Add(new EcsGenerateEventsSystem<EnemyReturnedToSpawnEvent>(_eventsWorld,
                        context._nonEcsEventCollector))
                    .Add(new PlayerGenerationSystem())
                    .Add(new AddOrUpdatePlayerDirectionSystem())
                    .Add(new AddOrUpdatePlayerAttackSystem())
                    .Add(new AddPlayerPositionSystem())
                    .Add(new AddPlayerViewSystem())
                    .Add(new ApplyPlayerViewChangesSystem())
                    .Add(new ApplyHudViewSystem())
                    .Add(new DeletePlayerOnDestroySystem())
                    .Add(new PlayerDeathHandlerSystem())
                    .Add(new EnemyGenerationSystem())
                    .Add(new AddEnemyViewSystem())
                    .Add(new AddOrDestroyMinigameViewSystem())
                    .Add(new AddOrRemoveNearestToPlayerMarkerSystem())
                    .Add(new UpdateEnemyStateSystem())
                    .Add(new UpdateEnemyDirectionSystem())
                    .Add(new DestroyReturnedEnemySystem())
                    .Add(new ApplyEnemyViewChangesSystem())
                    .Add(new ApplyMinigameViewChangesSystem())
                    .Add(new DeleteViewsOnDestroySystem())
                    .Inject(_inputService)
                    .Inject(_playerViewService)
                    .Inject(_hudViewService)
                    .Inject(_timeService)
                    .Inject(_randomService)
                    .Inject(_enemyViewService)
                    .Inject(_minigameViewService)
                    .Inject(_playerDeathHandlerService);

#if UNITY_EDITOR
                var debugSystem = new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem();
                var eventDebugSystem = new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem("events");
                _gameplayEcsSystems.Add(debugSystem);
                _gameplayEcsSystems.Add(eventDebugSystem);
#endif

                _gameplayEcsSystems.Init();
                _gameplayEcsSystems.Run();

                yield return context.FadeOutLoadingScreen();
                yield return context._popupManager.Show(_hudViewService.HudGamePopupPresenter);
            }

            public IState<GameController> OnUpdate(GameController context)
            {
                _gameplayEcsSystems.Run();

                if (_playerDeathHandlerService.WasDeathHandled || _inputService.ClickedPauseButton)
                    return new DeathState();
                return this;
            }

            public IEnumerator OnExitState(GameController context)
            {
                yield return context.FadeInLoadingScreen();
                yield return context._popupManager.Close(_hudViewService.HudGamePopupPresenter);

                _gameplayEcsSystems.Destroy();
                _world.Destroy();
                _eventsWorld.Destroy();

                _playerViewService.Dispose();
                _enemyViewService.Dispose();
                _minigameViewService.Dispose();
            }
        }
    }
}