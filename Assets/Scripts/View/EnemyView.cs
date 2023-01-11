using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Logic;
using Logic.Component;
using Logic.Component.Event;
using Logic.Extension.EcsGenerateEventsSystem;
using UnityEngine;
using View.Extensions;
using Vector2 = System.Numerics.Vector2;

namespace View
{
    [Serializable]
    public class EnemyViewVariantConfig
    {
        [field: SerializeField] public EnemyVariantValue EnemyVariantValue { get; private set; }
        [field: SerializeField] public EnemyViewVariant ViewVariant { get; private set; }
    }

    public interface IEnemyViewConfig
    {
        public Transform LeftSpawnPosition { get; }
        public Transform RightSpawnPosition { get; }
        public Transform NearPlayerLeftPosition { get; }
        public Transform NearPlayerRightPosition { get; }
        public float WalkingSpeed { get; }
        public float AngryWalkingSpeed { get; }
        public float ReturningSpeed { get; }
        public float WalkingAlpha { get; }
        public float AttackingAlpha { get; }
        public float ReturningAlpha { get; }
        public float AngryWalkingAlpha { get; }
    }

    public interface INonEcsEventCollectorHolder
    {
        public INonEcsEventCollector EventCollector { get; }
    }

    public sealed class EnemyView : MonoBehaviour, IEnemyView
    {
        [SerializeField] private EnemyViewVariantConfig[] _viewVariantConfigs;
        
        private Dictionary<EnemyVariantValue, EnemyViewVariant> _viewVariantConfigsLookup;
        private EnemyViewVariant _currentViewVariant;

        private static readonly int State = Animator.StringToHash("State");

        private Transform _transform;
        private IEnemyViewConfig _config;
        private INonEcsEventCollector _eventCollector;
        private InitialDirection _initialDirection;
        private Func<EnemyView, int> _getIdAction;

        private EnemyStateValue _currentEnemyState = EnemyStateValue.None;

        private Tween _activeTween; 

        public EnemyView ApplyEnemyViewConfig(
            IEnemyViewConfig config,
            INonEcsEventCollectorHolder eventCollectorHolder,
            Func<EnemyView, int> getIdAction
        ) 
        {
            _config = config;
            _eventCollector = eventCollectorHolder.EventCollector;
            _getIdAction = getIdAction;
            return this;
        }

        private void Awake()
        {
            _transform = transform;
            _viewVariantConfigsLookup = _viewVariantConfigs.ToDictionary(
                it => it.EnemyVariantValue,
                it => it.ViewVariant);
        }

        private Vector3 UnityPosition => _transform.position;
        
        private Vector3 UnitySpawnPosition =>
            (_initialDirection.IsLeft ? _config.RightSpawnPosition : _config.LeftSpawnPosition).position;
        
        private Vector3 UnityPlayerPosition =>
            (_initialDirection.IsLeft ? _config.NearPlayerRightPosition : _config.NearPlayerLeftPosition).position;

        Vector2 IEnemyView.Position => UnityPosition.ToNumericVector().ToVector2();
        
        Vector2 IEnemyView.SpawnPosition => UnitySpawnPosition.ToNumericVector().ToVector2();
        
        Vector2 IEnemyView.PlayerPosition => UnityPlayerPosition.ToNumericVector().ToVector2();

        void IEnemyView.ApplyInitialDirection(InitialDirection initialDirection)
        {
            _initialDirection = initialDirection;
            _transform.position =
                initialDirection.IsLeft ? _config.RightSpawnPosition.position : _config.LeftSpawnPosition.position;
        }

        void IEnemyView.ApplyDirection(Direction direction)
        {
            _currentViewVariant.SpriteRenderer.flipX = !direction.IsLeft;
        }

        void IEnemyView.ApplyEnemyState(EnemyState enemyState)
        {
            if (_currentEnemyState == enemyState.Value)
                return;
            _currentEnemyState = enemyState.Value;

            _currentViewVariant.Animator.SetInteger(State, enemyState.Value switch
            {
                EnemyStateValue.Walking => 0,
                EnemyStateValue.Attacking => 1,
                EnemyStateValue.Returning => 2,
                EnemyStateValue.AngryWalking => 2,
                _ => throw new Exception($"Unsupported case for '{enemyState.Value}'")
            });

            _currentViewVariant.SpriteRenderer.sortingLayerName = enemyState.Value switch
            {
                EnemyStateValue.Walking => "FocusedEnemy",
                EnemyStateValue.Attacking => "FocusedEnemy",
                EnemyStateValue.Returning => "UnfocusedEnemy",
                EnemyStateValue.AngryWalking => "FocusedEnemy",
                _ => throw new Exception($"Unsupported case for '{enemyState.Value}'")
            };

            var color = _currentViewVariant.SpriteRenderer.color;
            color.a = enemyState.Value switch
            {
                EnemyStateValue.Walking => _config.WalkingAlpha,
                EnemyStateValue.Attacking => _config.AttackingAlpha,
                EnemyStateValue.Returning => _config.ReturningAlpha,
                EnemyStateValue.AngryWalking => _config.AngryWalkingAlpha,
                _ => throw new Exception($"Unsupported case for '{enemyState.Value}'")
            };
            _currentViewVariant.SpriteRenderer.color = color;

            switch (enemyState.Value)
            {
                case EnemyStateValue.Walking:
                {
                    _activeTween?.Kill();
                    _activeTween = _transform
                        .DOMove(UnityPlayerPosition, duration: _config.WalkingSpeed)
                        .SetSpeedBased(true)
                        .OnComplete(() =>
                            _eventCollector.CollectEvent(new EnemyMovedToPlayerEvent { Enemy = _getIdAction(this) }))
                        .SetLink(gameObject, LinkBehaviour.KillOnDisable)
                        .SetUpdate(UpdateType.Manual)
                        .Play();
                    break;
                }
                case EnemyStateValue.Attacking:
                {
                    break;
                }
                case EnemyStateValue.AngryWalking:
                {
                    _activeTween?.Kill();
                    _activeTween = _transform
                        .DOMove(UnityPlayerPosition, duration: _config.AngryWalkingSpeed)
                        .SetSpeedBased(true)
                        .OnComplete(() =>
                            _eventCollector.CollectEvent(new EnemyMovedToPlayerEvent { Enemy = _getIdAction(this) }))
                        .SetLink(gameObject, LinkBehaviour.KillOnDisable)
                        .SetUpdate(UpdateType.Manual)
                        .Play();
                    break;
                }
                case EnemyStateValue.Returning:
                {
                    _activeTween?.Kill();
                    _activeTween = _transform
                        .DOMove(UnitySpawnPosition, duration: _config.ReturningSpeed)
                        .SetSpeedBased(true)
                        .OnComplete(() =>
                            _eventCollector.CollectEvent(new EnemyReturnedToSpawnEvent { Enemy = _getIdAction(this) }))
                        .SetLink(gameObject, LinkBehaviour.KillOnDisable)
                        .SetUpdate(UpdateType.Manual)
                        .Play();
                    break;
                }
                default:
                    throw new Exception($"Unsupported case for '{enemyState.Value}'");
            }
        }

        void IEnemyView.ApplyEnemyVariant(EnemyVariant enemyVariant)
        {
            _currentViewVariant = _viewVariantConfigsLookup[enemyVariant.Value];
            _currentViewVariant.gameObject.SetActive(true);
            foreach (var keyValuePair in _viewVariantConfigsLookup.Where(it => it.Key != enemyVariant.Value))
            {
                keyValuePair.Value.gameObject.SetActive(false);
            }
        }

        public void ApplyUpdate(ITimeService timeService)
        {
            _activeTween?.ManualUpdate(timeService.DeltaTime, timeService.UnscaledDeltaTime);
        }

        public void OnAttackAnimationFinished()
        {
            _eventCollector.CollectEvent(new EnemyAttackFinishedEvent { Enemy = _getIdAction(this) });
        }
    }
}