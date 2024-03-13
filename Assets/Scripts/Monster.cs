using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    class SlopeData
    {
        public bool onSlope;
        public Vector2 slopeVector;
    }
    public float moveSpeed;
    public float slopeDetectDistance;
    public LayerMask groundMask;
    public bool moveRight;

    private Rigidbody2D _rigidbody2d;
    private bool _onGround;
    private SlopeData _slope;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _onGround = DetectGround();
        _slope = DetectSlope();
        _rigidbody2d.velocity = decideMovement();
    }

    private SlopeData DetectSlope()
    {
        SlopeData slope = new SlopeData();
        RaycastHit2D hit = Physics2D.Raycast(
            _rigidbody2d.position, -Vector2.up, slopeDetectDistance, groundMask);
        slope.onSlope = Mathf.Abs(hit.normal.x) > 1e-4f;
        slope.slopeVector = Vector2.Perpendicular(hit.normal).normalized;
        return slope;
    }

    private bool DetectGround()
    {
        bool result = Physics2D.OverlapCircle(_rigidbody2d.position, 0.38f, groundMask);
        return result;
    }

    private Vector2 decideMovement()
    {
        if (moveSpeed == 0) { return _rigidbody2d.velocity; }
        Vector2 new_velocity = _rigidbody2d.velocity;
        new_velocity.x = moveSpeed * (moveRight ? 1f : -1f);
        if (IsOnGround()) {
            if (IsOnSlope()) {
                new_velocity = _slope.slopeVector * moveSpeed * (moveRight ? -1f : 1f);
            }
        }
        return new_velocity;
    }

    public bool IsOnGround() { return _onGround; }
    public bool IsOnSlope() { return _slope.onSlope; }
    public Vector2 GetSlopeVector() { return _slope.slopeVector; }
}
