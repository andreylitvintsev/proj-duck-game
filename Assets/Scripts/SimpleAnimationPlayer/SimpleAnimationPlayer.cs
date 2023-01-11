using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace SimpleAnimationPlayer
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator))]
    public class SimpleAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private AnimationClip _initialAnimationClip;
        [SerializeField] private bool _playOnStart;

        private PlayableGraph _playableGraph;

        public AnimationClip AnimationClip
        {
            get => _playableGraph.GetRootPlayableCount() > 0
                ? ((AnimationClipPlayable)_playableGraph.GetRootPlayable(0)).GetAnimationClip()
                : null;
            set => SetAnimationClip(value);
        }

        private void Awake()
        {
            _playableGraph = PlayableGraph.Create(name);
            _playableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            AnimationPlayableOutput.Create(_playableGraph, "ManInBar", GetComponent<Animator>());

            SetAnimationClip(_initialAnimationClip);
        }

        private void Start()
        {
            if (_playOnStart && AnimationClip != null)
                Play();
        }

        private void SetAnimationClip(AnimationClip animationClip)
        {
            if (_playableGraph.GetRootPlayableCount() != 0)
                _playableGraph.DestroyPlayable(_playableGraph.GetRootPlayable(0));
            if (animationClip != null)
            {
                var animationClipPlayable = AnimationClipPlayable.Create(_playableGraph, animationClip);
                if (!animationClip.isLooping)
                    animationClipPlayable.SetDuration(animationClip.length);
                _playableGraph.GetOutput(0).SetSourcePlayable(animationClipPlayable);
            }
        }

        public void Play()
        {
            if (AnimationClip != null)
                _playableGraph.Play();
        }

        public void Stop()
        {
            _playableGraph.Stop();
        }

        public bool IsPlaying => _playableGraph.IsValid() && !_playableGraph.IsDone();

        public IEnumerator WaitUntilPlayed => new WaitWhile(() => IsPlaying);

        private void OnDestroy()
        {
            _playableGraph.Destroy();
        }
    }
}