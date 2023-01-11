using Logic.Component;

namespace Logic
{
    public interface IHudViewService
    {
        public void ApplyPlayerHealth(PlayerHealth playerHealth);
        public void ApplyAttackedEnemiesCount(SuccessfulAttackCount successfulAttackCount);
    }
}