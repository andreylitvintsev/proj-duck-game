using System;
using Logic;
using Logic.Component;
using UnityEngine;

namespace View
{
    public sealed class PlayerView : MonoBehaviour, IPlayerView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;

        private Transform _transform;
        
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");

        private void Awake()
        {
            _transform = transform;
        }

        void IPlayerView.ApplyDirection(Direction direction)
        {
            _spriteRenderer.flipX = !direction.IsLeft;
        }

        void IPlayerView.ApplyIsAttacking(IsAttacking isAttacking)
        {
            _animator.SetBool(IsAttacking, isAttacking.Value);
        }

        void IPlayerView.ApplyPosition(Position position)
        {
            _transform.position = new Vector3(position.Value.X, position.Value.Y);
        }
    }
}