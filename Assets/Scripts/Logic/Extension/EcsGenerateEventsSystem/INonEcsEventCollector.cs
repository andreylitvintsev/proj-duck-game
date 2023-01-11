namespace Logic.Extension.EcsGenerateEventsSystem
{
    public interface INonEcsEventCollector
    {
        void CollectEvent<TEvent>(TEvent eventData) where TEvent : struct;
    }
}