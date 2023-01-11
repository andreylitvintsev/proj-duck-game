using Logic;
using Logic.Component;
using View.Popup;

namespace View
{
    public sealed class HudViewService : IHudViewService
    {
        public readonly HudGamePopupPresenter HudGamePopupPresenter;
        
        public HudViewService(int initialHealthValue)
        {
            HudGamePopupPresenter = new HudGamePopupPresenter(initialHealthValue);
        }

        public void ApplyPlayerHealth(PlayerHealth playerHealth)
        {
            HudGamePopupPresenter.CurrentPlayerHealth.Value = playerHealth.Value;
        }

        public void ApplyAttackedEnemiesCount(SuccessfulAttackCount successfulAttackCount)
        {
            HudGamePopupPresenter.AttackedEnemiesCount.Value = successfulAttackCount.Value;
        }
    }
}