using Logic.Component;

namespace Logic
{
    public interface IPlayerView
    {
        void ApplyPosition(Position position);
        void ApplyDirection(Direction direction);
        void ApplyIsAttacking(IsAttacking isAttacking);
    }
    
    public interface IPlayerViewService
    {
        Position SpawnPosition { get; }
        int InitialHealthValue { get; }

        public void ApplyEntityCreation(int id);
        public IPlayerView GetView(int id);
        public void ApplyEntityDestruction(int id);
    }
}