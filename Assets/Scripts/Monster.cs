using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public float moveSpeed;
    public float slopeDetectDistance;
    public LayerMask groundMask;
    public bool moveRight;

    private Rigidbody2D _rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        _rigidbody2d.velocity = decideMovement();
    }

    RaycastHit2D DetectSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            _rigidbody2d.position, -Vector2.up, slopeDetectDistance, groundMask);
        return hit;
    }

    bool isOnGround()
    {
        bool result = Physics2D.OverlapCircle(_rigidbody2d.position, 0.2f, groundMask);
        return result;
    }

    Vector2 decideMovement()
    {
        if (moveSpeed == 0) { return _rigidbody2d.velocity; }
        Vector2 new_velocity = _rigidbody2d.velocity;
        new_velocity.x = moveSpeed * (moveRight ? 1f : -1f);
        RaycastHit2D result_ray_hit = DetectSlope();
        if (isOnGround() && result_ray_hit) {
            Vector2 slope_normal_perpendicular = Vector2.Perpendicular(result_ray_hit.normal).normalized;
            // Debug.DrawRay(result_ray_hit.point, result_ray_hit.normal, Color.green);
            // Debug.DrawRay(result_ray_hit.point, slope_normal_perpendicular, Color.red);
            // on a slope
            if (Mathf.Abs(result_ray_hit.normal.x) > 1e-4f) {
                new_velocity = slope_normal_perpendicular * moveSpeed * (moveRight ? -1f : 1f);
            }
        }
        return new_velocity;
    }
}
