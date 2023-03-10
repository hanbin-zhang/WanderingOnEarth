using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;


public class EventController : IEnumerable<BaseEvent>
{
    private Dictionary<Type, BaseEvent> eventTypes = new Dictionary<Type, BaseEvent>();
    public IEnumerator<BaseEvent> GetEnumerator() => eventTypes.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => eventTypes.Values.GetEnumerator();
    public void Add(BaseEvent e) => eventTypes[e!.GetType()] = e!;

    public EventController() { }

#nullable enable
    public T? Get<T>() where T : BaseEvent
    {
        Type t = typeof(T);
        if (eventTypes.ContainsKey(t))
        {
            return (T)eventTypes[t];
        }
        Debug.LogError($"unregistered event type {t}");
        return default(T?);
    }
#nullable disable
}



