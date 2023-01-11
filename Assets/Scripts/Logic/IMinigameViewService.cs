using System.Numerics;

namespace Logic
{
    public interface IMinigameView
    {
        public void ApplyPosition(Vector2 position);
        public bool CanWin();
        public void ApplyUpdate(ITimeService timeService);
    }

    public interface IMinigameViewService
    {
        public void ApplyEntityCreation(int id);
        public IMinigameView GetView(int id);
        public void ApplyEntityDestruction(int id);
    }
}