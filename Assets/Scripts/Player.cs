using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using State;
using Actor;
using UnityEngine;
using UnityEngine.UI;

public class Player : ActorBase
{

    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _capsuleSize;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;

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

    public override void SetFriction(string friction_type)
    {
        if (friction_type == "full") {
            _capsuleCollider.sharedMaterial = fullFriction;
        } else if (friction_type == "none") {
            _capsuleCollider.sharedMaterial = noFriction;
        }
    }
    public override bool DetectGround()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        Vector2 check_position = _rigidbody.position;
        Collider2D hit = Physics2D.OverlapCircle(check_position, _capsuleSize.x, ground_mask);
        return hit != null && Mathf.Abs(_rigidbody.velocity.y) < 1e-4f;
    }
}
