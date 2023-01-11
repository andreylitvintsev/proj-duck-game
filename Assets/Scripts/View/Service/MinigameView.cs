using System;
using DG.Tweening;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using View.Extensions;
using Random = UnityEngine.Random;
using Vector2 = System.Numerics.Vector2;

namespace View
{
    public interface IMinigameViewConfig
    {
        public float GetMinigameRange(int instanceNumber);
        public float GetMinigameDuration(int instanceNumber);
    }
    
    public sealed class MinigameView : MonoBehaviour, IMinigameView
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Slider _leftSideSlider;
        [SerializeField] private Slider _rightSideSlider;
        [SerializeField] private Slider _valueSlider;
        
        [SerializeField] private RectTransform _movableObject;

        private IMinigameViewConfig _config;
        
        private Tween _activeTween;
        private float _fromWinRange;
        private float _toWinRange;

        private static int _instanceNumber = 0;
        
        public MinigameView ApplyMinigameViewConfig(IMinigameViewConfig config)
        {
            _config = config;
            ++_instanceNumber;
            // todo
            var range = _config.GetMinigameRange(_instanceNumber);
            _fromWinRange = Random.Range(0f, 1f - range);
            _toWinRange = _fromWinRange + range;
            //
            _leftSideSlider.value = _fromWinRange;
            _rightSideSlider.value = 1f - _toWinRange;
            // 
            var duration = _config.GetMinigameDuration(_instanceNumber);
            _activeTween = _valueSlider.DOValue(1f, duration).From(0f).SetLoops(-1, LoopType.Yoyo).SetUpdate(UpdateType.Manual);
            _activeTween.Goto(Random.Range(0f, duration), andPlay: true);
            return this;
        }

        private void OnDisable()
        {
            _activeTween?.Kill();
        }

        void IMinigameView.ApplyPosition(Vector2 position)
        {
            _movableObject.position = position.ToUnityVector();
        }

        bool IMinigameView.CanWin()
        {
            return _fromWinRange <= _valueSlider.value && _valueSlider.value <= _toWinRange;
        }

        void IMinigameView.ApplyUpdate(ITimeService timeService)
        {
            _activeTween?.ManualUpdate(timeService.DeltaTime, timeService.UnscaledDeltaTime);
        }
    }
}