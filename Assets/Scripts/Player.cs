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
    public Vector2 groundCastBoxSize;
    public Vector2 groundCastCenterOffset;
    public float groundCastDist;  // ground detect cast distance

    private void OnDrawGizmos()
    {
        Vector2 center = transform.position;
        center += groundCastCenterOffset;
        Gizmos.DrawWireCube(center - Vector2.up * groundCastDist, groundCastBoxSize);
    }

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

    protected override RaycastHit2D? DetectGround()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        Vector2 center = transform.position;
        center += groundCastCenterOffset;
        RaycastHit2D hit = Physics2D.BoxCast(center, 
            groundCastBoxSize, 0, -Vector2.up, groundCastDist, ground_mask);
        return hit ? hit : null; // check hit.collider is empty or not
    }

    protected override RaycastHit2D? DetectSlope()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        // TODO: now is hard coded, try to extract the parameter to unity property
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -Vector2.up, 1.0f, ground_mask);
        return hit ? hit : null; // check hit.collider is empty or not
    }
}
