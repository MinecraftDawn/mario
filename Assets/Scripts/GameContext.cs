using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class GameContext : MonoBehaviour
{
    public static EventQueue eventQueue;

    [SerializeField]
    private int score;

    void Start()
    {
        eventQueue = new EventQueue();
    }

    void Update()
    {
        if (!eventQueue.IsEmpty()) {
            EventBase game_event = eventQueue.Dequeue();
            game_event.UpdateContext(this);
        }
    }

    public void AddScore(int value) { score += value; }
}
