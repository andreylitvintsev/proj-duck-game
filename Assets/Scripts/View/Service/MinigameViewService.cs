using System;
using System.Collections.Generic;
using Logic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace View
{
    // TODO: обработка ошибок
    public sealed class MinigameViewService : IMinigameViewService, IDisposable
    {
        private readonly LinkedPool<MinigameView> _objectsPool;
        
        private readonly Dictionary<int, MinigameView> _idToView = new();

        public MinigameViewService(Canvas canvas, MinigameView minigameViewPrefab, IMinigameViewConfig viewConfig)
        {
            _objectsPool = new LinkedPool<MinigameView>(
                createFunc: () => Object.Instantiate(minigameViewPrefab, canvas.transform),
                actionOnGet: view => view.ApplyMinigameViewConfig(viewConfig).gameObject.SetActive(true),
                actionOnRelease: view => view.gameObject.SetActive(false),
                collectionCheck: Debug.isDebugBuild || Application.isEditor,
                maxSize: 100,
                actionOnDestroy: view => Object.Destroy(view.gameObject)
            );
        }

        public void ApplyEntityCreation(int id) 
        {
            _idToView[id] = _objectsPool.Get();
        }

        public IMinigameView GetView(int id)
        {
            return _idToView[id];
        }

        public void ApplyEntityDestruction(int id)
        {
            if (!_idToView.Remove(id, out var view))
                throw new Exception($"Passed '{nameof(id)}' not valid!");
            if (view != null)
                _objectsPool.Release(view);
        }

        public void Dispose()
        {
            _objectsPool.Dispose();
        }
    }
}