using System.Collections;
using PopupManager;
using UnityEngine;

namespace View.Popup
{
    public class PopupWithShowHideAnimations<TPopupPresenter> : Popup<TPopupPresenter>
        where TPopupPresenter : PopupPresenter
    {
        [SerializeField] private SimpleAnimationPlayer.SimpleAnimationPlayer _animationPlayer;
        [SerializeField] private AnimationClip _showAnimationClip;
        [SerializeField] private AnimationClip _hideAnimationClip;

        protected override IEnumerator OnBindPresenter(TPopupPresenter popupPresenter)
        {
            var animation = PlayAnimationClip(_showAnimationClip);
            yield return base.OnBindPresenter(popupPresenter);
            yield return animation;
        }

        protected override IEnumerator OnUnbindPresenter()
        {
            yield return PlayAnimationClip(_hideAnimationClip);
            yield return base.OnUnbindPresenter();
        }

        private IEnumerator PlayAnimationClip(AnimationClip animationClip)
        {
            _animationPlayer.Stop();

            if (animationClip == null)
                return null;

            _animationPlayer.AnimationClip = animationClip;
            _animationPlayer.Play();
            return _animationPlayer.WaitUntilPlayed;
        }
    }
}