using Logic.Extension.EcsGenerateEventsSystem;

namespace Logic.Component.Event
{
    public struct EnemyMovedToPlayerEvent : IStateCopyable<EnemyMovedToPlayerEvent>
    {
        public int Enemy;

        public void CopyState(EnemyMovedToPlayerEvent anotherObject)
        {
            Enemy = anotherObject.Enemy;
        }
    }
}