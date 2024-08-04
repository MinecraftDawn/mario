using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Mace : ItemBase
{
    [SerializeField]
    private float _forceFactor = 1f;
    private Rigidbody2D _rigidbody;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public override void Effect(ActorBase actor)
    {
        Player player = (Player)actor;
        player.Hurt(ComputeHitForce());
    }

    private Vector2 ComputeHitForce()
    {
        return _rigidbody.velocity * _forceFactor;
    }
}
