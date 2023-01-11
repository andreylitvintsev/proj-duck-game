using System;
using System.Collections;

namespace PopupManager
{
    public interface IPopup
    {
        Type PopupPresenterType { get; }
        IEnumerator Bind(PopupPresenter popupPresenter);
        IEnumerator Unbind();
    }
}