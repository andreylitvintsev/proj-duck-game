using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace PopupManager
{
    public class PopupManager : MonoBehaviour, IDisposable
    {
        [SerializeField] private GameObject[] popupObjectPrefabs;

        private readonly Dictionary<Type, LinkedPool<GameObject>> _popupPresenterTypeToPopupPool = new();

        private readonly Dictionary<PopupPresenter, GameObject> _popupPresenterToPopup = new();

        private bool _isDisposed = false;

        private void Awake()
        {
            foreach (var popupObjectPrefab in popupObjectPrefabs)
                RegisterPopupPool(GetPopupComponentOrThrow(popupObjectPrefab).PopupPresenterType, popupObjectPrefab);
        }

        public IEnumerator Show(PopupPresenter popupPresenter)
        {
            ThrowIfDisposed();
            
            popupPresenter.PopupManager = this;
            var popupObject = GetPopupPoolOrThrow(popupPresenter).Get();
            RegisterPopupPresenterForPopup(popupPresenter, popupObject);

            yield return GetPopupComponentOrThrow(popupObject).Bind(popupPresenter);
        }

        public IEnumerator Close(PopupPresenter popupPresenter)
        {
            ThrowIfDisposed();

            var popupComponent = GetPopupComponentOrThrow(popupPresenter);
            yield return popupComponent.Unbind();
            
            GetPopupPoolOrThrow(popupPresenter).Release(GetPopupObjectOrThrow(popupPresenter));
            UnregisterPopupPresenterForPopup(popupPresenter);
        }

        public bool IsActive(PopupPresenter popupPresenter)
        {
            ThrowIfDisposed();

            return _popupPresenterToPopup.ContainsKey(popupPresenter);
        }

        public void Dispose()
        {
            foreach (var keyValuePair in _popupPresenterToPopup)
                GetPopupPoolOrThrow(keyValuePair.Key).Release(keyValuePair.Value);
            _popupPresenterToPopup.Clear();
            
            foreach (var popupPool in _popupPresenterTypeToPopupPool.Values)
                popupPool.Dispose();
            
            _isDisposed = true;
        }

        #region Helpers

        private void RegisterPopupPool(Type popupPresenterType, GameObject popupObjectPrefab)
        {
            if (_popupPresenterTypeToPopupPool.ContainsKey(popupPresenterType))
                throw new Exception($"Popup for type '{popupPresenterType.Name}' already registered!");
            _popupPresenterTypeToPopupPool[popupPresenterType] = new LinkedPool<GameObject>(
                createFunc: () => Instantiate(popupObjectPrefab, transform),
                actionOnDestroy: pooledPopupObject => Destroy(pooledPopupObject.gameObject)
            );
        }

        private void RegisterPopupPresenterForPopup(PopupPresenter popupPresenter, GameObject popupObject)
        {
            if (_popupPresenterToPopup.ContainsKey(popupPresenter))
                throw new Exception($"Popup presenter '{popupPresenter}' already registered for popup object '{popupObject.name}'!");
            _popupPresenterToPopup[popupPresenter] = popupObject;
        }

        private void UnregisterPopupPresenterForPopup(PopupPresenter popupPresenter)
        {
            if (!_popupPresenterToPopup.Remove(popupPresenter))
                throw new Exception($"Popup presenter '{popupPresenter}' not registered!");
        }
        
        private IPopup GetPopupComponentOrThrow(GameObject popupObject)
        {
            if (!popupObject.TryGetComponent<IPopup>(out var popupComponent))
                throw new Exception($"Passed object '{popupObject.name}' is not popup!");
            return popupComponent;
        }

        private IPopup GetPopupComponentOrThrow(PopupPresenter popupPresenter)
        {
            return GetPopupComponentOrThrow(GetPopupObjectOrThrow(popupPresenter));
        }

        private GameObject GetPopupObjectOrThrow(PopupPresenter popupPresenter)
        {
            if (!_popupPresenterToPopup.TryGetValue(popupPresenter, out var popupObject))
                throw new Exception($"Popup with given presenter '{popupPresenter}' not registered!");
            return popupObject;
        }

        private LinkedPool<GameObject> GetPopupPoolOrThrow(PopupPresenter popupPresenter)
        {
            return GetPopupPoolOrThrow(popupPresenter.GetType());
        }
        
        private LinkedPool<GameObject> GetPopupPoolOrThrow(Type popupPresenterType)
        {
            if (!_popupPresenterTypeToPopupPool.TryGetValue(popupPresenterType, out var popupObjectsPool))
                throw new Exception($"Popup pool for type '{popupPresenterType.FullName}' is not registered!");
            return popupObjectsPool;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new Exception($"You can't use destroyed '{GetType().FullName}'");
        }

        #endregion
    }
}