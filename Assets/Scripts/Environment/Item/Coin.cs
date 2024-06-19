using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Coin : ItemBase
{
    public int coinScore = 1;

    public override void Effect(ActorBase actor)
    {
        Debug.Log("Coin Get daze!");
        GameContext.eventQueue.Enqueue(new Event.GetCoin(coinScore));
        Destroy(gameObject);
    }
}
