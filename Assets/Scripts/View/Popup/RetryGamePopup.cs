using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using PopupManager;
using UnityEngine;
using UnityEngine.UI;

namespace View.Popup
{
    public class RetryGamePopup : Popup<RetryGamePopupPresenter>
    {
        [SerializeField] private Button _retryGameButton;
        [SerializeField] private Button _toMainMenuButton;

        private RetryGamePopupPresenter _popupPresenter;
        
        protected override IEnumerator OnBindPresenter(RetryGamePopupPresenter popupPresenter)
        {
            _popupPresenter = popupPresenter;
            _retryGameButton.onClick.AddListener(_popupPresenter.OnRetryGameButtonClick);
            _toMainMenuButton.onClick.AddListener(_popupPresenter.OnToMainMenuButtonClick);
            return base.OnBindPresenter(popupPresenter);
        }

        protected override IEnumerator OnUnbindPresenter()
        {
            _retryGameButton.onClick.RemoveListener(_popupPresenter.OnRetryGameButtonClick);
            _toMainMenuButton.onClick.RemoveListener(_popupPresenter.OnToMainMenuButtonClick);
            return base.OnUnbindPresenter();
        }
    }

    public class RetryGamePopupPresenter : PopupPresenter
    {
        public enum ButtonClickType
        {
            None, RetryGame, ToMainMenu
        }

        public ButtonClickType ClickedButton { get; private set; } = ButtonClickType.None; 
        
        public void OnRetryGameButtonClick()
        {
            ClickedButton = ButtonClickType.RetryGame;
        }

        public void OnToMainMenuButtonClick()
        {
            ClickedButton = ButtonClickType.ToMainMenu;
        }
    }
}