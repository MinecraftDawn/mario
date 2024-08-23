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
    protected HashSet<BaseCommand> _commandHistoryInLastCycle;
    protected HashSet<BaseCommand> _commandHistoryInCurrentCycle;
    protected HashSet<BaseCommand> _commandSet;
    protected Rigidbody2D _rigidbody;
    protected bool _onGround;
    protected bool _onSlope;
    protected bool _isFalling;
    protected LayerMask _groundMask;
    protected Vector2 _groundDirection;
    protected CommandPool _commandPool;
    protected StateManager _stateManager;
    public float horizontalSpeed = 3f;
    public float jumpForce = 3f;
    public float maxFallSpeed = 10f;
    public float groundCastDist;  // ground detect cast distance
    public Vector2 groundCastBoxSize;
    public Vector2 groundCastCenterOffset;
    public Vector2 directionCastBoxSize = new Vector2(0.3f, 0.4f);

    /************************************************************
    * Major Method
    *************************************************************/

    private void OnDrawGizmos()
    {
        Vector2 center = transform.position;
        center += groundCastCenterOffset;
        Gizmos.DrawWireCube(center - Vector2.up * groundCastDist, groundCastBoxSize);
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        _stateManager = new StateManager();
        InitialState();
        _commandSet = new HashSet<BaseCommand>();
        _commandHistoryInCurrentCycle = new HashSet<BaseCommand>();
        _commandHistoryInLastCycle = new HashSet<BaseCommand>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundMask = LayerMask.GetMask("Ground");
        CommandPool.InitPool();
        _commandPool = new CommandPool();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        BaseState old_state = _stateManager.GetCurrentState();
        BaseState current_state = _stateManager.GetCurrentState().Update(this);
        if (!ReferenceEquals(old_state, current_state)) { current_state.OnStateStart(this); }
    }

    public virtual void FixedUpdate()
    {
        // TODO: Consider to call detect function in Update.
        PreparationBeforeFixedUpdate();
        StateFixedUpdate();
    }

    private void StateFixedUpdate()
    {
        BaseState old_state = _stateManager.GetCurrentState();
        BaseState current_state = old_state.StatusCheck(this);
        if (!ReferenceEquals(old_state, current_state)) { current_state.OnStateStart(this); }

        old_state = _stateManager.GetCurrentState();
        current_state = old_state.FixedUpdate(this);
        if (!ReferenceEquals(old_state, current_state)) { current_state.OnStateStart(this); }

        CleanCommandList();
        UpdateCommandHistory();
    }

    protected virtual void CollectState()
    {
        DetectGround();
        DetectSlope();
        DetectGroundDirection();
        _isFalling = IsStateType<InAirState>() && _rigidbody.velocity.y <= 0f;
    }

    protected virtual void PreparationBeforeFixedUpdate() { CollectState(); }

    /************************************************************
    * Detection Method
    ************************************************************/

    protected virtual RaycastHit2D? DetectGround() 
    {
        Vector2 center = transform.position;
        center += groundCastCenterOffset;
        RaycastHit2D hit = Physics2D.BoxCast(center, 
            groundCastBoxSize, 0, -Vector2.up, groundCastDist, _groundMask);
        _onGround = hit;
        return hit ? hit : null; // check hit.collider is empty or not
    }

    protected virtual RaycastHit2D? DetectGroundDirection()
    {
        Vector2 center = transform.position;
        center += groundCastCenterOffset + 
                  new Vector2(directionCastBoxSize.x / 2, 0f) * Mathf.Sign(velocity.x);
        RaycastHit2D hit;
        hit = Physics2D.BoxCast(
            center, directionCastBoxSize, 0, -Vector2.up, 0.3f, _groundMask);
        if (hit) {
            _groundDirection = -Vector2.Perpendicular(hit.normal).normalized;
            Debug.DrawRay(hit.point, hit.normal, Color.green);
            Debug.DrawRay(hit.point, _groundDirection, Color.red);
        }
        return hit ? hit : null;
    }

    protected virtual RaycastHit2D? DetectSlope() 
    {
        _onSlope = false;
        // TODO: now is hard coded, try to extract the parameter to unity property
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -Vector2.up, 1.0f, _groundMask);
        _onSlope = hit && Mathf.Abs(hit.normal.x) > 1e-4f;
        return hit ? hit : null; // check hit.collider is empty or not
    }

    /************************************************************
    * State Related Method
    ************************************************************/
    
    protected virtual void InitialState() { Debug.Log("[Warning] InitialState no override!"); }
    public Type GetStateType() { return _stateManager.GetCurrentState().GetType(); }
    public State StateTransition<State>() where State : BaseState, new() { return _stateManager.StateTransition<State>(); }
    public bool IsStateType<State>() { return _stateManager.GetCurrentState() is State; }

    /************************************************************
    * Command Related Method
    ************************************************************/

    public virtual bool IsContainCommand<Command>() where Command : BaseCommand
    {
        return _commandSet.Contains(_commandPool.GetSample<Command>());
    }

    public virtual bool IsContainCommandInLastCycle<Command>() where Command : BaseCommand
    {
        return _commandHistoryInLastCycle.Contains(_commandPool.GetSample<Command>());
    }

    public virtual bool ExecuteCommand<Command>() where Command : BaseCommand
    {
        BaseCommand command;
        bool search = _commandSet.TryGetValue(_commandPool.GetSample<Command>(), out command);
        if (!search) { return false; }
        if (command == null) { return false; }
        
        command.Execute(this);
        _commandSet.Remove(command);
        _commandHistoryInCurrentCycle.Add(command);
        return true;
    }

    public T GenerateCommand<T>() where T : BaseCommand
    {
        T command = _commandPool.RequestObject<T>();
        return command;
    }

    protected virtual void UpdateCommandHistory()
    {
        _commandHistoryInLastCycle.Clear();
        foreach(BaseCommand executed_command in _commandHistoryInCurrentCycle) {
            _commandHistoryInLastCycle.Add(executed_command);
        }
        _commandHistoryInCurrentCycle.Clear();
    }

    public virtual int GetCommandListSize() { return _commandSet.Count; }
    public virtual void CleanCommandList() { _commandSet.Clear(); }
    public virtual IEnumerable<BaseCommand> GetCommandListEnumerable() { return _commandSet; }
    public virtual void ReceiveCommands(BaseCommand command) { _commandSet.Add(command); }

    /************************************************************
    * Getter/Setter and Boolean Status Check Method
    ************************************************************/

    public virtual void SetFriction(FrictionType friction_type) {}
    public Vector2 velocity {
        get { return _rigidbody.velocity; }
        set { _rigidbody.velocity = value; }
    }
    public Rigidbody2D GetRigidbody() { return _rigidbody; }
    public Vector2 GetGroundDirection() { return _groundDirection; }
    public bool IsOnGround() { return _onGround; }
    public bool IsOnSlope() { return _onSlope; }
    public bool IsFalling() { return _isFalling; }

    /************************************************************
    * Others
    ************************************************************/

    public virtual void FlipObject()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    public virtual Vector2 ObjectFaceDirection() { return transform.localScale.x > 0.0f ? Vector2.right : Vector2.left; }
    public virtual GameObject GetNewProjectile() { return null; }

}
}