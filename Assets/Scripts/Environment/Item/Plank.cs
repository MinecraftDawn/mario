using System.Collections;
using System.Collections.Generic;
using Actor;
using Envrionment.Projectile;
using UnityEngine;
using utils;

public class Plank : ItemBase
{
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float _triggerCoolDown = 3f;
    [SerializeField]
    private DelayTimer _triggerCoolDownTimer;
    private bool _canTrigger;
    private ProjectileManager _projectileManager;
    private Vector2 _direction;
    [SerializeField]
    private GameObject _firePosition;
    [SerializeField]
    private float _firePositionOffsetX;
    [SerializeField]
    private float _firePositionOffsetY;
    [SerializeField]
    private float _projectileSpeed;
    private Animator _animator;



    void Start()
    {
        _triggerCoolDownTimer = new DelayTimer(_triggerCoolDown);
        _canTrigger = true;
        _projectileManager = new ProjectileManager(projectile);
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!_canTrigger && _triggerCoolDownTimer.HasDelayPassed()) { _canTrigger = true; }
    }

    public override void Effect(ActorBase actor)
    {
        if (_canTrigger) { 
            _canTrigger = false;
            _triggerCoolDownTimer.UpdateLastTime();

            _animator.SetTrigger("PlankTrigger");

            _firePosition.transform.position = new Vector2(transform.position.x + _firePositionOffsetX, transform.position.y + _firePositionOffsetY);
            Vector2 player_position = actor.transform.position;
            _direction = (player_position - (Vector2)_firePosition.transform.position).normalized;

            projectile = _projectileManager.GetInstance();
            if (projectile == null) { Debug.Log("[Error] projectile is null"); }
            Rigidbody2D projectile_rigid = projectile.GetComponent<Rigidbody2D>();
            projectile.transform.position = _firePosition.transform.position;
            projectile_rigid.velocity = _direction * _projectileSpeed;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        ActorBase actor = other.GetComponent<ActorBase>();
        Effect(actor);
    }
}
