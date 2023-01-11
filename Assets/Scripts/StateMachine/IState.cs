using System.Collections;

namespace StateMachine
{
    public interface IState<in TContext>
    {
        IEnumerator OnEnterState(TContext context);

        IState<TContext> OnUpdate(TContext context) => this;

        IEnumerator OnExitState(TContext context);
    }
}