using System;
using System.Collections;
using System.Collections.Generic;
using PopupManager;
using SimpleReactiveProperty;
using TMPro;
using UnityEngine;

namespace View.Popup
{
    public sealed class HudGamePopup : PopupWithShowHideAnimations<HudGamePopupPresenter>
    {
        [Header("HudGamePopup")]
        [SerializeField] private Transform _healthItemViewHolder;
        [SerializeField] private HealthItemView _healthItemViewScenePrefab;
        [SerializeField] private TMP_Text _attackedEnemiesCountText;

        private readonly List<HealthItemView> _healthItemViews = new();
        private Action _onUnbind;

        protected override IEnumerator OnBindPresenter(HudGamePopupPresenter popupPresenter)
        {
            InitializeHealthItemViews(popupPresenter);
            StartListeningProperties(popupPresenter);
            return base.OnBindPresenter(popupPresenter);
        }

        protected override IEnumerator OnUnbindPresenter()
        {
            StopListeningProperties();
            DestroyHealthItemViews();
            return base.OnUnbindPresenter();
        }

        private void InitializeHealthItemViews(HudGamePopupPresenter popupPresenter)
        {
            var prefabGameObject = _healthItemViewScenePrefab.gameObject;
            prefabGameObject.SetActive(true);
            for (int i = 0; i < popupPresenter.InitialHealthValue.Value; ++i)
                _healthItemViews.Add(Instantiate(_healthItemViewScenePrefab, _healthItemViewHolder));
            prefabGameObject.SetActive(false);
        }

        private void StartListeningProperties(HudGamePopupPresenter popupPresenter)
        {
            _onUnbind += popupPresenter.CurrentPlayerHealth
                .SubscribeOnValueChanged(OnHealthValueChanged, notifyOnSubscribe: true)
                .Dispose;
            _onUnbind += popupPresenter.AttackedEnemiesCount
                .SubscribeOnValueChanged(OnAttackedEnemiesCountChanged, notifyOnSubscribe: true)
                .Dispose;
        }

        private void StopListeningProperties()
        {
            _onUnbind.Invoke();
            _onUnbind = null;
        }

        private void DestroyHealthItemViews()
        {
            foreach (var healthItemView in _healthItemViews)
                Destroy(healthItemView.gameObject);
            _healthItemViews.Clear();
        }
        
        private void OnHealthValueChanged(int newValue)
        {
            for (var i = 0; i < _healthItemViews.Count; i++)
                _healthItemViews[i].IsAvailable = i + 1 <= newValue;
        }

        private void OnAttackedEnemiesCountChanged(int newValue)
        {
            _attackedEnemiesCountText.SetText("{0}", newValue);
        }
    }

    public class HudGamePopupPresenter : PopupPresenter
    {
        public readonly ReactiveProperty<int> InitialHealthValue;
        public readonly ReactiveProperty<int> CurrentPlayerHealth;
        public readonly ReactiveProperty<int> AttackedEnemiesCount;
        
        public HudGamePopupPresenter(int totalPlayerHealth)
        {
            InitialHealthValue = new ReactiveProperty<int>(totalPlayerHealth);
            CurrentPlayerHealth = new ReactiveProperty<int>(totalPlayerHealth);
            AttackedEnemiesCount = new ReactiveProperty<int>(0);
        }
    }
}