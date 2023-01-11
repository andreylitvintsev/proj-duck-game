using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using StateMachine;
using View.Popup;

namespace View
{
    public sealed partial class GameController
    {
        private sealed class MainMenuState : IState<GameController>
        {
            private readonly StartGamePopupPresenter _startGamePopupPresenter;
            
            public MainMenuState()
            {
                _startGamePopupPresenter = new StartGamePopupPresenter();
            }

            public IEnumerator OnEnterState(GameController context)
            {
                yield return context._popupManager.Show(_startGamePopupPresenter);
                yield return context.FadeOutLoadingScreen();
            }

            public IState<GameController> OnUpdate(GameController context)
            {
                if (_startGamePopupPresenter.IsStartGameClicked)
                    return new GameState();
                return this;
            }

            public IEnumerator OnExitState(GameController context)
            {
                yield return context.FadeInLoadingScreen();
                yield return context._popupManager.Close(_startGamePopupPresenter);
            }
        }
    }
}