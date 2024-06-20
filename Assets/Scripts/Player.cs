using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using State;
using Actor;
using enums;
using UnityEngine;
using UnityEngine.UI;

public class Player : ActorBase
{

    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _capsuleSize;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;
    public int health = 3;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleSize = _capsuleCollider.size;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MonsterBody") { Hurt(); }
    }

    public override void SetFriction(FrictionType friction_type)
    {
        if (friction_type == FrictionType.FULL) {
            _capsuleCollider.sharedMaterial = fullFriction;
        } else if (friction_type == FrictionType.NONE) {
            _capsuleCollider.sharedMaterial = noFriction;
        }
    }

    protected override BaseState InitialState() { return new OnLandState(); }

    public void Hurt()
    {
        health -= 1;
        if (health <= 0) { GameContext.eventQueue.Enqueue(new Event.PlayerDead()); }
    }
}
