using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using State;
using Actor;
using enums;
using UnityEngine;

public class Player : ActorBase
{

    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private float _linearDrag = 4f;
    [SerializeField]
    private float _fallMultiplier = 3f;
    [SerializeField]
    // This attribute is used to compute player's movement speed.
    // It is calculated independently, and will overwrite the rigidbody velocity in some rule.
    // For detail implementation see AddForce method in below.
    private float _moveSpeed = 0f; 
    [SerializeField]
    private float _accelerate = 5f;
    [SerializeField]
    private float _decelerate = 3f;
    private CapsuleCollider2D _capsuleCollider;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;
    public int health = 3;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
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

    public void AddMovementForce(float force)
    {
        // formula: f = ma, a = f / m
        _moveSpeed = _moveSpeed + Time.deltaTime * (force / _rigidbody.mass);
        if (IsStateType<OnLandState>() && IsOnSlope()) {
            velocity = _moveSpeed * GetGroundDirection();
        } else {
            velocity = new Vector2(_moveSpeed, velocity.y);
        }
    }

    public void NoDrag() { _rigidbody.drag = 0; }
    public void ResetDrag() { _rigidbody.drag = _linearDrag; }
    public void NoGravity() { _rigidbody.gravityScale = 0; }
    public void SetGravityToFull() { _rigidbody.gravityScale = _gravity * _fallMultiplier; }
    public void SetGravityToBase() { _rigidbody.gravityScale = _gravity; }
    public void SetGravityToHalf() { _rigidbody.gravityScale = _gravity * (_fallMultiplier / 2); }
    public void SetGravityToZero() { _rigidbody.gravityScale = 0; }
    public float GetMoveSpeed() { return _moveSpeed; }
    public float GetAccelerate() { return _accelerate; }
    public float GetDecelerate() { return _decelerate; }

    protected override BaseState InitialState() { return new OnLandState(); }
}
