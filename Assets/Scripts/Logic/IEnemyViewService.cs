using System.Numerics;
using Logic.Component;

namespace Logic
{
    public interface IEnemyView
    {
        Vector2 Position { get; }
        Vector2 SpawnPosition { get; }
        Vector2 PlayerPosition { get; }
        
        void ApplyInitialDirection(InitialDirection initialDirection);
        void ApplyDirection(Direction direction);
        void ApplyEnemyState(EnemyState enemyState);
        void ApplyEnemyVariant(EnemyVariant enemyVariant);
        void ApplyUpdate(ITimeService timeService);
    }
    
    public interface IEnemyViewService
    {
        float GetDelayToGenerate(int enemyNumber);

        void ApplyEntityCreation(int id);
        IEnemyView GetView(int id);
        void ApplyEntityDestruction(int id);
    }
}