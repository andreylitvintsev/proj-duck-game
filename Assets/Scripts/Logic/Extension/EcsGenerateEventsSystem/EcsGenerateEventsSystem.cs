using Leopotam.EcsLite;

namespace Logic.Extension.EcsGenerateEventsSystem
{
    public class EcsGenerateEventsSystem<TEvent> : IEcsRunSystem
        where TEvent : struct, IStateCopyable<TEvent>
    {
        private readonly EcsWorld _world;
        private readonly NonEcsEventCollector _nonEcsEventCollector;
        private readonly EcsFilter _entityFilter;

        public EcsGenerateEventsSystem(EcsWorld world, NonEcsEventCollector nonEcsEventCollector)
        {
            _world = world;
            _nonEcsEventCollector = nonEcsEventCollector;
            _entityFilter = _world.Filter<TEvent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var eventEntity in _entityFilter)
                _world.DelEntity(eventEntity);
            _nonEcsEventCollector.FlushEventsIntoWorld<TEvent>(_world);
        }
    }
}