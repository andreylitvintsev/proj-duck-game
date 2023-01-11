using System;
using System.Collections;
using StateMachine;
using View.Popup;

namespace View
{
    public sealed partial class GameController
    {
        public sealed class DeathState : IState<GameController>
        {
            private readonly RetryGamePopupPresenter _retryGamePopupPresenter;

            public DeathState()
            {
                _retryGamePopupPresenter = new RetryGamePopupPresenter();
            }

            public IEnumerator OnEnterState(GameController context)
            {
                yield return context._popupManager.Show(_retryGamePopupPresenter);
                yield return context.FadeOutLoadingScreen();
            }

            public IState<GameController> OnUpdate(GameController context)
            {
                return _retryGamePopupPresenter.ClickedButton switch
                {
                    RetryGamePopupPresenter.ButtonClickType.None => this,
                    RetryGamePopupPresenter.ButtonClickType.RetryGame => new GameState(),
                    RetryGamePopupPresenter.ButtonClickType.ToMainMenu => new MainMenuState(),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            public IEnumerator OnExitState(GameController context)
            {
                yield return context.FadeInLoadingScreen();
                yield return context._popupManager.Close(_retryGamePopupPresenter);
            }
        }
    }
}