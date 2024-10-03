using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Goal : ItemBase
{
    public override void Effect(ActorBase actor)
    {
        Debug.Log("Goallllllllllllllll!");
        GameContext.eventQueue.Enqueue(new Event.GameFinish());
    }
}
