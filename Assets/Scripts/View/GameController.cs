using System.Collections;
using Logic.Extension.EcsGenerateEventsSystem;
using StateMachine;
using UnityEngine;
using View.Popup;

namespace View
{
    public sealed partial class GameController : MonoBehaviour,
        IEnemyViewConfig, IMinigameViewConfig, INonEcsEventCollectorHolder
    {
        // TODO: сделать ограничение на прокидывание префабов
        [Header("Dependencies")]
        [SerializeField] private PlayerView _playerViewPrefab;
        [SerializeField] private EnemyView _enemyViewPrefab;
        [SerializeField] private Canvas _minigamesCanvas;
        [SerializeField] private MinigameView _minigameViewPrefab;
        [SerializeField] private PopupManager.PopupManager _popupManager;
        
        [Space]
        [Header("Game Balance")]
        [SerializeField] private Transform _playerSpawnPoint;
        [SerializeField] private Transform _leftEnemiesSpawnPoint;
        [SerializeField] private Transform _rightEnemiesSpawnPoint;
        [SerializeField] private Transform _leftPlayerPoint;
        [SerializeField] private Transform _rightPlayerPoint;
        [SerializeField] private AnimationCurve _delayToGenerateEnemyChart;
        [SerializeField] private AnimationCurve _numberToMinigameCompletionRange;
        [SerializeField] private AnimationCurve _numberToMinigameDurationSpeed;
        [SerializeField, Min(1)] private int _initialPlayerHealthValue = 3;
        [SerializeField, Min(0f)] private float _walkingSpeed = 0.5f;
        [SerializeField, Min(0f)] private float _angryWalkingSpeed = 1f;
        [SerializeField, Min(0f)] private float _returningSpeed = 0.5f;
        [SerializeField, Min(0f)] private float _walkingAlpha = 1f;
        [SerializeField, Min(0f)] private float _attackingAlpha = 1f;
        [SerializeField, Min(0f)] private float _returningAlpha = 0.4f;
        [SerializeField, Min(0f)] private float _angryWalkingAlpha = 1f;

        private readonly NonEcsEventCollector _nonEcsEventCollector = new();
        private StateMachine<GameController> _stateMachine;

        private readonly LoadingPopupPresenter _loadingPopupPresenter = new();
        
        #region IEnemyViewConfig

        Transform IEnemyViewConfig.LeftSpawnPosition => _leftEnemiesSpawnPoint;
        
        Transform IEnemyViewConfig.RightSpawnPosition => _rightEnemiesSpawnPoint;
        
        Transform IEnemyViewConfig.NearPlayerLeftPosition => _leftPlayerPoint;
        
        Transform IEnemyViewConfig.NearPlayerRightPosition => _rightPlayerPoint;

        #endregion

        #region IMinigameViewConfig

        float IMinigameViewConfig.GetMinigameRange(int instanceNumber) =>
            _numberToMinigameCompletionRange.Evaluate(instanceNumber);
        
        float IMinigameViewConfig.GetMinigameDuration(int instanceNumber) =>
            _numberToMinigameDurationSpeed.Evaluate(instanceNumber);

        float IEnemyViewConfig.WalkingSpeed => _walkingSpeed;

        float IEnemyViewConfig.AngryWalkingSpeed => _angryWalkingSpeed;

        float IEnemyViewConfig.ReturningSpeed => _returningSpeed;
        
        float IEnemyViewConfig.WalkingAlpha => _walkingAlpha;
        
        float IEnemyViewConfig.AttackingAlpha => _attackingAlpha;

        float IEnemyViewConfig.ReturningAlpha => _returningAlpha;
        
        float IEnemyViewConfig.AngryWalkingAlpha => _angryWalkingAlpha;

        #endregion

        #region INonEcsEventCollectorHolder

        INonEcsEventCollector INonEcsEventCollectorHolder.EventCollector => _nonEcsEventCollector;
        
        #endregion

        private void Start()
        {
            _stateMachine = new StateMachine<GameController>(this, new MainMenuState());
            StartCoroutine(_stateMachine.Update());
        }

        private void OnDestroy()
        {
            _stateMachine.Dispose();
            if (_popupManager != null)
                _popupManager.Dispose();
        }

        private IEnumerator FadeInLoadingScreen()
        {
            yield return _popupManager.Show(_loadingPopupPresenter);
        }

        private IEnumerator FadeOutLoadingScreen()
        {
            if (_popupManager.IsActive(_loadingPopupPresenter))
                yield return _popupManager.Close(_loadingPopupPresenter);
        }
    }
}