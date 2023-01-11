using Logic.Extension.EcsGenerateEventsSystem;

namespace Logic.Component.Event
{
    public struct EnemyReturnedToSpawnEvent : IStateCopyable<EnemyReturnedToSpawnEvent>
    {
        public int Enemy;

        public void CopyState(EnemyReturnedToSpawnEvent anotherObject)
        {
            Enemy = anotherObject.Enemy;
        }
    }
}