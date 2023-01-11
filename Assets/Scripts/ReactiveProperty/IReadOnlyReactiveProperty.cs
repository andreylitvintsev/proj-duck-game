using System;

namespace SimpleReactiveProperty
{
    public interface IReadOnlyReactiveProperty<out TValue>
    {
        public TValue Value { get; }
        public IDisposable SubscribeOnValueChanged(Action<TValue> onEvent, bool notifyOnSubscribe = false);
    }
}