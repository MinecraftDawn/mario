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

    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private float _linearDrag = 4f;
    [SerializeField]
    private float _fallMultiplier = 3f;
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
        if (other.gameObject.tag == "MonsterBody") {
            health -= 1;
        }
        if (health <= 0) { GameContext.eventQueue.Enqueue(new Event.PlayerDead()); }
    }

    public override void SetFriction(FrictionType friction_type)
    {
        if (friction_type == FrictionType.FULL) {
            _capsuleCollider.sharedMaterial = fullFriction;
        } else if (friction_type == FrictionType.NONE) {
            _capsuleCollider.sharedMaterial = noFriction;
        }
    }

    public void NoDrag() { _rigidbody.drag = 0; }
    public void ResetDrag() { _rigidbody.drag = _linearDrag; }
    public void NoGravity() { _rigidbody.gravityScale = 0; }
    public void SetGravityToFull() { _rigidbody.gravityScale = _gravity * _fallMultiplier; }
    public void SetGravityToBase() { _rigidbody.gravityScale = _gravity; }
    public void SetGravityToHalf() { _rigidbody.gravityScale = _gravity * (_fallMultiplier / 2); }

    protected override BaseState InitialState() { return new OnLandState(); }
}
