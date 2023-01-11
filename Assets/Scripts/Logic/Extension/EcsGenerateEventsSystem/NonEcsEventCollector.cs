using System;
using System.Collections;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Logic.Extension.EcsGenerateEventsSystem
{
    public class NonEcsEventCollector : INonEcsEventCollector
    {
        private readonly Dictionary<Type, IList> _dictionary = new();

        public void CollectEvent<TEvent>(TEvent eventData) where TEvent : struct
        {
            if (!_dictionary.ContainsKey(eventData.GetType()))
                _dictionary[eventData.GetType()] = new List<TEvent>();
            var events = (List<TEvent>) _dictionary[eventData.GetType()];
            events.Add(eventData);
        }

        public void FlushEventsIntoWorld<TEvent>(EcsWorld world) where TEvent : struct, IStateCopyable<TEvent>
        {
            if (!_dictionary.TryGetValue(typeof(TEvent), out var events))
                return;
            
            foreach (var nonEcsEvent in (List<TEvent>) events)
                world.GetPool<TEvent>().Add(world.NewEntity()).CopyState(nonEcsEvent);
            events.Clear();
        }
    }
}