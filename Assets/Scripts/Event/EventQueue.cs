using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Event
{

public class EventQueue
{
    private Queue<EventBase> _queue;
    public EventQueue() { _queue = new Queue<EventBase>(); }
    public void Enqueue(EventBase game_event) { _queue.Enqueue(game_event); }
    public EventBase Dequeue() { return _queue.Dequeue(); }
    public int Size() { return _queue.Count; }
    public bool IsEmpty() { return _queue.Count == 0; }
}

}
