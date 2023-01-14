using Logic;
using UnityEngine;

namespace View
{
    public sealed class InputService : IInputService
    {
        public bool PressedToRight =>
            Input.GetKey(KeyCode.RightArrow) || (Input.GetMouseButton(0) && RightScreenArea.Contains(Input.mousePosition));
        public bool PressedToLeft =>
            Input.GetKey(KeyCode.LeftArrow) || (Input.GetMouseButton(0) && LeftScreenArea.Contains(Input.mousePosition));

        public bool ClickedToRight => 
            Input.GetKey(KeyCode.RightArrow) || (Input.GetMouseButtonDown(0) && RightScreenArea.Contains(Input.mousePosition));
        public bool ClickedToLeft =>
            Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetMouseButtonDown(0) && LeftScreenArea.Contains(Input.mousePosition));

        public bool ClickedPauseButton => Input.GetKeyDown(KeyCode.Escape);

        private static Rect LeftScreenArea => new(0f, 0f, Screen.width / 2f, Screen.height);
        private static Rect RightScreenArea => new(Screen.width / 2f, 0f, Screen.width / 2f, Screen.height);
    }
}