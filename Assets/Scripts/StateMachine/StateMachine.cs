using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine<TContext> : IDisposable
    {
        private readonly TContext _context; 
            
        private IState<TContext> _currentState;
        private IState<TContext> _nextState;
        private bool _isDisposed = false;

        public StateMachine(TContext context, IState<TContext> initialState)
        {
            _context = context;
            _nextState = initialState;
        }

        public IEnumerator Update()
        {
            ThrowIfDisposed();

            while (Application.isPlaying && !_isDisposed)
            {
                if (!ReferenceEquals(_currentState, _nextState))
                {
                    _currentState = _nextState;
                    yield return _currentState.OnEnterState(_context);
                }

                _nextState = _currentState.OnUpdate(_context);

                if (!ReferenceEquals(_currentState, _nextState))
                { 
                    yield return _currentState.OnExitState(_context);
                }

                yield return null;
            }
        }

        public void Dispose()
        {
            ThrowIfDisposed();

            _isDisposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException($"You can't use destroyed '{GetType().FullName}'");
        }
    }
}