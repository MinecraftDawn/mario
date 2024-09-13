using System;
using System.Collections;
using System.Collections.Generic;
using Command;
using State;
using Actor;
using enums;
using UnityEngine;
using utils;

public class Player : ActorBase
{

    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private float _linearDrag = 4f;
    [SerializeField]
    private float _fallMultiplier = 3f;
    [SerializeField]
    // This attribute is used to compute player's movement speed.
    // It is calculated independently, and will overwrite the rigidbody velocity in some rule.
    // For detail implementation see AddForce method in below.
    private float _moveSpeed = 0f; 
    [SerializeField]
    private float _accelerate = 5f;
    [SerializeField]
    private float _decelerate = 3f;
    [SerializeField]
    private DelayTimer _invincibleTimer;
    [SerializeField]
    private float _unmoveTimeWhenHurt = 0.5f;
    [SerializeField]
    private UI.HealthBarController _healthBar;
    [SerializeField]
    private Color _hurtEffectColor;
    private LayerMask _invincibleExcludeLayer;
    private LayerMask _originalExcludeMask;
    private bool _isInvincible = false;
    private CapsuleCollider2D _capsuleCollider;
    private SpriteRenderer _spriteRenderer;
    public PhysicsMaterial2D fullFriction;
    public PhysicsMaterial2D noFriction;
    public int health = 3;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _invincibleExcludeLayer = ~LayerMask.GetMask("Ground") & ~LayerMask.GetMask("Item");
        _originalExcludeMask = _capsuleCollider.excludeLayers;
        _healthBar.Init(health);
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "MonsterBody") {
            // Debug.Log("Hurt by monster!");
            Hurt(other.gameObject.GetComponent<Monster.Monster>().ComputeHitForce(this));
        }
        
    }

    public override void SetFriction(FrictionType friction_type)
    {
        if (friction_type == FrictionType.FULL) {
            _capsuleCollider.sharedMaterial = fullFriction;
        } else if (friction_type == FrictionType.NONE) {
            _capsuleCollider.sharedMaterial = noFriction;
        }
    }

    public void Hurt(Vector2 force)
    {
        health -= 1;
        if (health <= 0) { GameContext.eventQueue.Enqueue(new Event.PlayerDead()); }
        velocity = Vector2.zero;
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
        StateTransition<UnmovableState>();
        _stateManager.GetCurrentState().OnStateStart(this);
        SetInvincible();
        _healthBar.DecreaseHealth(health);
    }
    
    public void AddMovementForce(float force)
    {
        // formula: f = ma, a = f / m
        _moveSpeed = _moveSpeed + Time.deltaTime * (force / _rigidbody.mass);
        // if ((IsStateType<OnLandState>() || IsStateType<UnmovableState>()) && IsOnSlope()) {
        if (IsOnGround() && IsOnSlope()) {
            velocity = _moveSpeed * GetGroundDirection();
        } else {
            velocity = new Vector2(_moveSpeed, velocity.y);
        }
    }

    public void NoDrag() { _rigidbody.drag = 0; }
    public void ResetDrag() { _rigidbody.drag = _linearDrag; }
    public void NoGravity() { _rigidbody.gravityScale = 0; }
    public void SetGravityToFull() { _rigidbody.gravityScale = _gravity * _fallMultiplier; }
    public void SetGravityToBase() { _rigidbody.gravityScale = _gravity; }
    public void SetGravityToHalf() { _rigidbody.gravityScale = _gravity * (_fallMultiplier / 2); }
    public void SetGravityToZero() { _rigidbody.gravityScale = 0; }
    public void CleanMoveSpeed() { _moveSpeed = 0f; }
    public void SyncMoveSpeedWithVelocityX() { _moveSpeed = velocity.x; }
    public float GetMoveSpeed() { return _moveSpeed; }
    public float GetAccelerate() { return _accelerate; }
    public float GetDecelerate() { return _decelerate; }
    public float GetUnmoveTime() { return _unmoveTimeWhenHurt; }
    public bool IsInvincible() { return _isInvincible; }
    public void SetInvincible() { StartCoroutine(StartInvincible()); }

    private IEnumerator StartInvincible()
    {
        _isInvincible = true;
        _invincibleTimer.UpdateLastTime();
        _capsuleCollider.excludeLayers = _invincibleExcludeLayer;
        Color original_color = _spriteRenderer.color;
        while (!_invincibleTimer.HasDelayPassed()) {
            _spriteRenderer.color = _hurtEffectColor;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = original_color;
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("remove invincible");
        _isInvincible = false;
        _capsuleCollider.excludeLayers = _originalExcludeMask;
        _spriteRenderer.color = original_color;
    }

    public override void ReceiveCommands(BaseCommand command)
    {
        if (_commandSet.Contains(command)) {
            _commandPool.ReturnObject(command);
            return;
        }
        base.ReceiveCommands(command);
    }

    protected override void InitialState()
    {
        _stateManager.Init<OnLandState>();
        // _stateManager.Init<UnmovableState>();
    }
    protected override void UpdateCommandHistory()
    {
        foreach (BaseCommand history_command in _commandHistoryInLastCycle) {
            _commandPool.ReturnObject(history_command);
        }
        base.UpdateCommandHistory();
    }

    public override void CleanCommandList()
    {
        foreach (BaseCommand command in _commandSet) {
            _commandPool.ReturnObject(command);
        }
        base.CleanCommandList();
    }

    public Vector3 GetObjectCenter()
    {
        Vector3 center = transform.position;
        center += (Vector3)_capsuleCollider.offset;
        return center;
    }

}
