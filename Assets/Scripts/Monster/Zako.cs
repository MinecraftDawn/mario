using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;

namespace Monster
{

public class Zako : Monster
{
    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _capsuleSize;

    public override void Start()
    {
        base.Start();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleSize = _capsuleCollider == null ? Vector2.zero : _capsuleCollider.size;
    }

    protected override void CollectState()
    {
        base.CollectState();
        _frontWall = DetectFrontWall() != null;
    }

    protected override void InitialState() { _stateManager.Init<MonsterOnLandState>(); }

    protected RaycastHit2D? DetectFrontWall()
    {
        Vector2 start_position = _rigidbody.position;
        Vector2 direction = Vector2.right * (GetMoveToRight() ? 1 : -1);
        start_position.y += _capsuleSize.y / 2;
        float ray_length = _capsuleSize.x / 2 + frontWallDetectRayLength;
        RaycastHit2D hit = Physics2D.Raycast(start_position, direction, ray_length, _groundMask);
        Debug.DrawRay(start_position, direction * ray_length, Color.red);
        return hit ? hit : null;
    }
}

}
