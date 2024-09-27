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
    [SerializeField]
    private float _degreeOfFloorRay; 

    public override void Start()
    {
        base.Start();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleSize = _capsuleCollider == null ? Vector2.zero : _capsuleCollider.size;
    }

    protected override void InitialState() { _stateManager.Init<MonsterOnLandState>(); }

    protected override RaycastHit2D? DetectFrontWall()
    {
        Vector2 start_position = _rigidbody.position;
        Vector2 direction = Vector2.right * (GetMoveToRight() ? 1 : -1);
        start_position.y += _capsuleSize.y / 2;
        float ray_length = _capsuleSize.x / 2 + frontWallDetectRayLength;
        RaycastHit2D hit = Physics2D.Raycast(start_position, direction, ray_length, _groundMask);
        Debug.DrawRay(start_position, direction * ray_length, Color.red);
        return hit ? hit : null;
    }

    protected override RaycastHit2D? DetectFrontFloor()
    {
        Vector2 start_position = _rigidbody.position;
        start_position.y += _capsuleSize.y / 2;
        Vector2 ray_direction = Vector2.zero;
        if (GetMoveToRight()) {
            ray_direction = Quaternion.Euler(0, 0, -_degreeOfFloorRay) * Vector2.right;
        } else {
            ray_direction = Quaternion.Euler(0, 0, _degreeOfFloorRay) * -Vector2.right;
        }
        RaycastHit2D hit = Physics2D.Raycast(start_position, ray_direction, 1, _groundMask);
        Debug.DrawRay(start_position, ray_direction, Color.red);
        return hit ? hit : null;
    }
}

}
