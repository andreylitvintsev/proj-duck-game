using System;

namespace Logic
{
    public interface ITimeService
    {
        public float DeltaTime { get; }
        public float UnscaledDeltaTime { get; }
    }
}