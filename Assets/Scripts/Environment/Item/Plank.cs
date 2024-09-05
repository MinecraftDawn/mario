using System.Collections;
using System.Collections.Generic;
using Actor;
using Envrionment.Projectile;
using UnityEngine;
using utils;

public class Plank : ItemBase
{
    public GameObject projectile;
    [SerializeField]
    private float _triggerCoolDown = 3f;
    [SerializeField]
    private DelayTimer _triggerCoolDownTimer;
    private bool _canTrigger;
    private ProjectileManager _projectileManager;
    private Vector2 _direction;
    private Vector2 _firePosition;
    [SerializeField]
    private float _firePositionOffsetX;
    [SerializeField]
    private float _firePositionOffsetY;
    [SerializeField]
    private float _projectileSpeed;

    void Start()
    {
        _triggerCoolDownTimer = new DelayTimer(_triggerCoolDown);
        _triggerCoolDownTimer.UpdateLastTime();
        _canTrigger = true;
        _projectileManager = new ProjectileManager(projectile);
    }

    void Update()
    {
        if (!_canTrigger && _triggerCoolDownTimer.HasDelayPassed()) { _canTrigger = true; }
    }

    public override void Effect(ActorBase actor)
    {
        if (_canTrigger) { 
            Debug.Log("Trigger the plank trap");
            _canTrigger = false;
            _triggerCoolDownTimer.UpdateLastTime();

            _firePosition = new Vector2(transform.position.x + _firePositionOffsetX, transform.position.y + _firePositionOffsetY);
            Vector2 player_position = actor.transform.position;
            _direction = (player_position - _firePosition).normalized;

            projectile = _projectileManager.GetInstance();
            if (projectile == null) { Debug.LogError("[Error] projectile is null"); }
            Rigidbody2D projectile_rigid = projectile.GetComponent<Rigidbody2D>();
            projectile.transform.position = _firePosition;
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
