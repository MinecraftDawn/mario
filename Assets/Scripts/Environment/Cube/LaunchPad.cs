using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    [SerializeField]
    private float _launchForce;
    private bool _canLaunch;
    private Rigidbody2D _rigidbody;

    void Awake()
    {
        // To ensure the AddFore is executed after rigidbody is awake,
        // wait for the first FixedUpdate.
        _canLaunch = true;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Kill itself when the item is destroyed
        if (transform.childCount != 0) { return; }
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (!_canLaunch) { return; }
        _rigidbody.AddForce(_launchForce * Vector2.up, ForceMode2D.Impulse);
        _canLaunch = false;
    }
}
