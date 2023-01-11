using System.Collections;
using PopupManager;
using UnityEngine;
using UnityEngine.UI;

namespace View.Popup
{
    public sealed class StartGamePopup : Popup<StartGamePopupPresenter>
    {
        [SerializeField] private Button _startGameButton;

        private StartGamePopupPresenter _popupPresenter;

        protected override IEnumerator OnBindPresenter(StartGamePopupPresenter popupPresenter)
        {
            _popupPresenter = popupPresenter;
            _startGameButton.onClick.AddListener(OnStartGameButtonClick);
            return base.OnBindPresenter(popupPresenter);
        }

        private void OnStartGameButtonClick()
        {
            _popupPresenter.OnStartGameButtonClick();
        }

        protected override IEnumerator OnUnbindPresenter()
        {
            _startGameButton.onClick.RemoveListener(OnStartGameButtonClick);
            return base.OnUnbindPresenter();
        }
    }

    public class StartGamePopupPresenter : PopupPresenter
    {
        public bool IsStartGameClicked { get; private set; } = false;

        public void OnStartGameButtonClick()
        {
            IsStartGameClicked = true;
        }
    }
}