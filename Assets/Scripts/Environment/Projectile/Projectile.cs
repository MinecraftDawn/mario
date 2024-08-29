using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;

namespace Environment
{
namespace Projectile
{

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected DelayTimer _liveTime;
    [SerializeField]
    protected float _hitForce = 1f;
    protected Rigidbody2D _rigidbody;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _liveTime.UpdateLastTime();
    }

    void Update()
    {
        if (!_liveTime.HasDelayPassed()) { return; }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") { return; }
        Player player = other.gameObject.GetComponent<Player>();
        player.Hurt(_rigidbody.velocity.normalized * _hitForce);
    }
}

}
}
