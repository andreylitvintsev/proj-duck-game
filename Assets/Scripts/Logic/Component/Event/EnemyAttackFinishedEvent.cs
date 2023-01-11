using Logic.Extension.EcsGenerateEventsSystem;

namespace Logic.Component.Event
{
    public struct EnemyAttackFinishedEvent : IStateCopyable<EnemyAttackFinishedEvent>
    {
        public int Enemy;

        public void CopyState(EnemyAttackFinishedEvent anotherObject)
        {
            Enemy = anotherObject.Enemy;
        }
    }
}