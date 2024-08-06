using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MaceChain : MonoBehaviour
{
    [SerializeField]
    private GameObject _chainBody;
    [SerializeField]
    private float _maxAngle = 45.0f;
    [SerializeField]
    private float _gravity = 9.81f;
    private Rigidbody2D _chainBodyRigidbody;
    private float _length;
    private float direction = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _chainBodyRigidbody = _chainBody.GetComponent<Rigidbody2D>();
        _length = _chainBody.GetComponent<BoxCollider2D>().size.y;
    }

    void FixedUpdate()
    {
        float currentAngle = _chainBody.transform.eulerAngles.z * Mathf.Deg2Rad;
        float angle = _maxAngle * Mathf.Deg2Rad;
        if (currentAngle > angle && currentAngle < 2f * Mathf.PI - angle) { direction *= -1f; }
        // Non-linear pendulum, the max angle exceed 5 degree, angular velocity will changed.
        // angular velocity = sqrt(2 * g / L * (cos(max_angle) - cos(current_angle)))
        // where g is gravity, L is the length from mace to pivot
        _chainBodyRigidbody.angularVelocity =
            Mathf.Sqrt((2 * _gravity) / _length * Mathf.Abs(Mathf.Cos(angle) - Mathf.Cos(currentAngle))) * Mathf.Rad2Deg;
        _chainBodyRigidbody.angularVelocity *= direction;
    }
}
