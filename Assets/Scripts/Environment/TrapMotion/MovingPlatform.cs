using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace TrapMotion
{

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float move_speed = 3f;
    [SerializeField]
    private List<Transform> _moveTargets;
    private Rigidbody2D _rigidbody;
    private int _moveTowardIdx;
    private int _idxDirection;

    void OnDrawGizmos()
    {
        Transform previous_transform = _moveTargets[0];
        for (int i = 1; i < _moveTargets.Count; i++) {
            Gizmos.DrawLine(previous_transform.position, _moveTargets[i].position);
            previous_transform = _moveTargets[i];
        }
    }

    void Start()
    {
        transform.position = _moveTargets[0].position;
        _moveTowardIdx = 1;
        _idxDirection = 1;
        _rigidbody = GetComponent<Rigidbody2D>();
        SetupVelocity();
    }

    void Update()
    {
        Transform target_transform = _moveTargets[_moveTowardIdx];
        float distance = (target_transform.position - transform.position).magnitude;
        if (distance <= 0.1f) { TowardNextTarget(); }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            player._platformRigidbody = _rigidbody;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player") {
            Player player = other.gameObject.GetComponent<Player>();
            player._platformRigidbody = null;
        }
    }

    void TowardNextTarget()
    {
        if (_moveTowardIdx == _moveTargets.Count - 1 && _idxDirection > 0) {
            _idxDirection = -1;
        } else if (_moveTowardIdx == 0 && _idxDirection < 0) {
            _idxDirection = 1;
        }
        _moveTowardIdx += _idxDirection;
        SetupVelocity();
    }

    void SetupVelocity()
    {
        Transform next_target = _moveTargets[_moveTowardIdx];
        Transform previous_target = _moveTargets[_moveTowardIdx - _idxDirection];
        Vector2 direction_to_target = (next_target.position - previous_target.position).normalized;
        _rigidbody.velocity = direction_to_target * move_speed;
    }
}

}
