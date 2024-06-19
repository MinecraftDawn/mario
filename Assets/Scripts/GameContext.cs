using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class GameContext : MonoBehaviour
{
    public static EventQueue eventQueue;
    public int maxExecuteCount = 5;

    [SerializeField]
    private int score;

    void Start()
    {
        eventQueue = new EventQueue();
    }

    void Update()
    {
        int count = 0;
        while (!eventQueue.IsEmpty() && count < maxExecuteCount) {
            EventBase game_event = eventQueue.Dequeue();
            game_event.UpdateContext(this);
            count++;
        }
    }

    public void AddScore(int value) { score += value; }
}
