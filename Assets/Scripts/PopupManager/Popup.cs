using System;
using System.Collections;
using UnityEngine;

namespace PopupManager
{
    public abstract class Popup<TPopupPresenter> : MonoBehaviour, IPopup where TPopupPresenter : PopupPresenter
    {
        #region IPopup

        Type IPopup.PopupPresenterType => typeof(TPopupPresenter);
        
        IEnumerator IPopup.Bind(PopupPresenter popupPresenter)
        {
            yield return OnBindPresenter((TPopupPresenter) popupPresenter);
        }
        
        IEnumerator IPopup.Unbind()
        {
            yield return OnUnbindPresenter();
        }
        
        #endregion

        protected virtual IEnumerator OnBindPresenter(TPopupPresenter popupPresenter)
        {
            gameObject.SetActive(true);
            yield break;
        }

        protected virtual IEnumerator OnUnbindPresenter()
        {
            gameObject.SetActive(false);
            yield break;
        }
    }
}