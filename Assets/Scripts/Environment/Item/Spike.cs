using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Spike : ItemBase
{
    [SerializeField]
    private float _hitForce = 1f;

    public override void Effect(ActorBase actor)
    {
        Player player = (Player)actor;
        player.Hurt(computeHitForce(player));
    }

    private Vector2 computeHitForce(Player player)
    {
        float face_to = player.transform.localScale.x;
        Vector2 force_direction = new Vector2(face_to, 1f).normalized;
        return force_direction * _hitForce;
    }
}
