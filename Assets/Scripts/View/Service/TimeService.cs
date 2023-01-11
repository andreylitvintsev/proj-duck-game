using Logic;
using UnityEngine;

namespace View
{
    public sealed class TimeService : ITimeService
    {
        public float DeltaTime => Time.deltaTime;
        public float UnscaledDeltaTime => Time.unscaledTime;
    }
}