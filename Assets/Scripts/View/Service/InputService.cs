using Logic;
using UnityEngine;

namespace View
{
    public sealed class InputService : IInputService
    {
        public bool PressedToRight => Input.GetKey(KeyCode.RightArrow);
        public bool PressedToLeft => Input.GetKey(KeyCode.LeftArrow);

        public bool ClickedToRight => Input.GetKeyDown(KeyCode.RightArrow);
        public bool ClickedToLeft => Input.GetKeyDown(KeyCode.LeftArrow);

        public bool ClickedPauseButton => Input.GetKeyDown(KeyCode.Escape);
    }
}