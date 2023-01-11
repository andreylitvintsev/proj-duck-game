using System;
using System.Collections.Generic;
using Logic;
using Logic.Component;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace View
{
    //  TODO: добавить проверки и exception
    public sealed class PlayerViewService : IPlayerViewService, IDisposable
    {
        private readonly LinkedPool<PlayerView> _objectsPool; //todo перечитай про пулы

        private readonly Dictionary<int, PlayerView> _idToView = new();

        public Position SpawnPosition { get; }

        public int InitialHealthValue { get; }

        public PlayerViewService(PlayerView playerViewPrefab, Vector2 spawnPosition, int initialHealthValue)
        {
            _objectsPool = new LinkedPool<PlayerView>(
                createFunc: () => Object.Instantiate(playerViewPrefab),
                actionOnGet: view => view.gameObject.SetActive(true),
                actionOnRelease: view => view.gameObject.SetActive(false),
                collectionCheck: Debug.isDebugBuild || Application.isEditor,
                maxSize: 10,
                actionOnDestroy: view => Object.Destroy(view.gameObject)
            );
            SpawnPosition = new Position
            {
                Value = new System.Numerics.Vector2(spawnPosition.x, spawnPosition.y)
            };
            InitialHealthValue = initialHealthValue;
        }

        public void ApplyEntityCreation(int id)
        {
            _idToView.Add(id, _objectsPool.Get());
        }

        public IPlayerView GetView(int id)
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