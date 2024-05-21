using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : ItemBase
{
    public override void Effect()
    {
        Debug.Log("Goallllllllllllllll!");
        GameContext.eventQueue.Enqueue(new Event.GameFinish());
    }
}
