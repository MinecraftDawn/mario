using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Spike : ItemBase
{
    public override void Effect(ActorBase actor)
    {
        Player player = (Player)actor;
        player.Hurt();
    }
}
