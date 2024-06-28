using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Command;
using enums;
using State;
using UnityEngine;

namespace Actor {
public abstract class ActorBase : MonoBehaviour {
    protected BaseState _state;
    protected HashSet<BaseCommand> _commandHistoryInLastCycle;
    protected HashSet<BaseCommand> _commandHistoryInCurrentCycle;
    protected HashSet<BaseCommand> _commandSet;
    protected Rigidbody2D _rigidbody;
    protected bool _onGround;
    protected bool _onSlope;
    protected bool _isFalling;
    protected Vector2 _groundDirection;
    public float horizontalSpeed = 3f;
    public float jumpForce = 3f;
    public float maxFallSpeed = 10f;
    public float groundCastDist;  // ground detect cast distance
    public Vector2 groundCastBoxSize;
    public Vector2 groundCastCenterOffset;

    private void OnDrawGizmos()
    {
        Vector2 center = transform.position;
        center += groundCastCenterOffset;
        Gizmos.DrawWireCube(center - Vector2.up * groundCastDist, groundCastBoxSize);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        _state = InitialState();
        _commandSet = new HashSet<BaseCommand>();
        _commandHistoryInCurrentCycle = new HashSet<BaseCommand>();
        _commandHistoryInLastCycle = new HashSet<BaseCommand>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        _state = _state.Update(this);
    }

    public virtual void FixedUpdate()
    {
        PreparationBeforeFixedUpdate();
        StateFixedUpdate();
    }

    private void StateFixedUpdate()
    {
        BaseState oldState = _state;
        _state = _state.FixedUpdate(this);
        if (!ReferenceEquals(oldState, _state)) { _state.OnStateStart(this); }
        CleanCommandList();
        UpdateCommandHistory();
    }

    protected virtual void CollectState()
    {
        DetectGround();
        DetectSlope();
        _isFalling = IsStateType<InAirState>() && _rigidbody.velocity.y < 0f;
    }

    protected virtual RaycastHit2D? DetectGround() 
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        Vector2 center = transform.position;
        center += groundCastCenterOffset;
        RaycastHit2D hit = Physics2D.BoxCast(center, 
            groundCastBoxSize, 0, -Vector2.up, groundCastDist, ground_mask);
        _onGround = hit;
        if (hit) {
            _groundDirection = -Vector2.Perpendicular(hit.normal).normalized;
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, _groundDirection, Color.red);
        }
        return hit ? hit : null; // check hit.collider is empty or not
    }

    protected virtual RaycastHit2D? DetectSlope() 
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        _onSlope = false;
        // TODO: now is hard coded, try to extract the parameter to unity property
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -Vector2.up, 1.0f, ground_mask);
        _onSlope = hit && Mathf.Abs(hit.normal.x) > 1e-4f;
        return hit ? hit : null; // check hit.collider is empty or not
    }

    public virtual void FlipObject()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    public virtual Vector2 ObjectFaceDirection() { return transform.localScale.x > 0.0f ? Vector2.right : Vector2.left; }
    protected virtual BaseState InitialState() { return null; }
    protected virtual void PreparationBeforeFixedUpdate() { CollectState(); }
    public virtual void SetFriction(FrictionType friction_type) {}
    public Vector2 velocity {
        get { return _rigidbody.velocity; }
        set { _rigidbody.velocity = value; }
    }
    public Rigidbody2D GetRigidbody() { return _rigidbody; }
    public bool IsOnGround() { return _onGround; }
    public bool IsOnSlope() { return _onSlope; }
    public bool IsFalling() { return _isFalling; }
    public Type GetStateType() { return _state.GetType(); }

    public bool IsStateType<State>() { return _state is State; }
    public Vector2 GetGroundDirection() { return _groundDirection; }

    public virtual bool IsContainCommand<Command>() { return _commandSet.Any(x => x is Command); }
    public virtual int GetCommandListSize() { return _commandSet.Count; }
    public virtual void CleanCommandList() {_commandSet.Clear(); }
    public virtual IEnumerable<BaseCommand> GetCommandListEnumable() { return _commandSet; }
    public virtual void ReceiveCommands(BaseCommand command) { _commandSet.Add(command); }

    public virtual bool ExecuteCommand<Command>() {
        BaseCommand? command = _commandSet.FirstOrDefault(x => x is Command);
        if (command is null) { return false; }
        
        command.Execute(this);
        _commandHistoryInCurrentCycle.Add(command);
        return true;
    }

    protected virtual void UpdateCommandHistory()
    {
        _commandHistoryInLastCycle.Clear();
        foreach(BaseCommand executed_command in _commandHistoryInCurrentCycle) {
            _commandHistoryInLastCycle.Add(executed_command);
        }
        _commandHistoryInCurrentCycle.Clear();
    }
}
}