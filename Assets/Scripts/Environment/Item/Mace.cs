using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Mace : ItemBase
{
    public override void Effect(ActorBase actor) {
        Player player = (Player)actor;
        player.Hurt();
    }
}
