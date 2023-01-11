using Logic;
using Logic.System;

namespace View
{
    public sealed class PlayerDeathHandlerService : IPlayerDeathHandlerService
    {
        public bool WasDeathHandled { get; private set; } = false;

        public void HandleDeath() => WasDeathHandled = true;
    }
}