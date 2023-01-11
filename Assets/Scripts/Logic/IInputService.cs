namespace Logic
{
    public interface IInputService
    {
        public bool PressedToRight { get; }
        public bool PressedToLeft { get; }
        
        public bool ClickedToRight { get; }
        public bool ClickedToLeft { get; }
        
        public bool ClickedPauseButton { get; }
    }
}