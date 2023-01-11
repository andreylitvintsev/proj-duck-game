using System.Collections;

namespace PopupManager
{
    public abstract class PopupPresenter
    {
        internal PopupManager PopupManager;
        
        public IEnumerator ClosePopup()
        {
            yield return PopupManager.Close(this);
        }
    }
}