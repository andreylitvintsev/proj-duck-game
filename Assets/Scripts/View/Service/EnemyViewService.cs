using System;
using System.Collections.Generic;
using Logic;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace View
{
    // TODO: добавить исключения
    public sealed class EnemyViewService : IEnemyViewService, IDisposable
    {
        private readonly LinkedPool<EnemyView> _objectsPool;
        private readonly AnimationCurve _delayToGenerateChart;
        
        private readonly Dictionary<int, EnemyView> _idToView = new();
        private readonly Dictionary<EnemyView, int> _viewToId = new();

        public EnemyViewService(
            EnemyView enemyViewPrefab, 
            IEnemyViewConfig viewConfig,
            INonEcsEventCollectorHolder eventCollectorHolder,
            AnimationCurve delayToGenerateChart
        )
        {
            _objectsPool = new LinkedPool<EnemyView>(
                createFunc: () => Object.Instantiate(enemyViewPrefab),
                actionOnGet: view =>
                    view.ApplyEnemyViewConfig(viewConfig, eventCollectorHolder, GetIdFromView)
                        .gameObject.SetActive(true),
                actionOnRelease: view => view.gameObject.SetActive(false),
                collectionCheck: Debug.isDebugBuild || Application.isEditor,
                maxSize: 100,
                actionOnDestroy: view => Object.Destroy(view.gameObject)
            );
            _delayToGenerateChart = delayToGenerateChart;
        }

        private int GetIdFromView(EnemyView view)
        {
            return _viewToId[view]; // TODO: обработать ошибку
        }

        public float GetDelayToGenerate(int enemyNumber)
        {
            return _delayToGenerateChart.Evaluate(enemyNumber);
        }

        public void ApplyEntityCreation(int id)
        {
            var view = _objectsPool.Get();
            _idToView.Add(id, view);
            _viewToId.Add(view, id);
        }

        public IEnemyView GetView(int id)
        {
            return _idToView[id];
        }

        public void ApplyEntityDestruction(int id)
        {
            if (!_idToView.Remove(id, out var view))
                throw new Exception($"Passed '{nameof(id)}' not valid!");
            
            if (!_viewToId.Remove(view, out id))
                throw new Exception($"Passed view not valid!");
            
            if (view != null)
                _objectsPool.Release(view);
        }

        public void Dispose()
        {
            _objectsPool.Dispose();
        }
    }
}