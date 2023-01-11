using Leopotam.EcsLite;

namespace Logic.Component
{
    public enum EnemyStateValue
    {
        None, Walking, Attacking, AngryWalking, Returning
    }
    
    public struct EnemyState : IEcsAutoReset<EnemyState>
    {
        public EnemyStateValue Value;

        public void AutoReset(ref EnemyState enemyState)
        {
            enemyState.Value = EnemyStateValue.None;
        }
    }
}