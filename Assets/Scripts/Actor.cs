using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Command;
using State;
using UnityEngine;

namespace Actor {
public abstract class ActorBase : MonoBehaviour {
    protected BaseState _state;
    protected List<BaseCommand> _commandList;
    protected Rigidbody2D _rigidbody;
    protected bool _onGround;
    protected bool _onSlope;
    protected Vector2 _groundDirection;
    public float horizontalSpeed = 3f;
    public float jumpForce = 3f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        _state = new OnLandState();
        _commandList = new List<BaseCommand>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        _state = _state.Update(gameObject);
    }

    public virtual void FixedUpdate()
    {
        CollectState();
        BaseState oldState = _state;
        _state = _state.FixedUpdate(gameObject);
        if (!ReferenceEquals(oldState, _state)) { _state.OnStateStart(gameObject); }
    }

    // TODO: maybe define the function in the child class rather than here.
    protected virtual void CollectState()
    {
        _onGround = DetectGround() != null;
        _onSlope = false;
        RaycastHit2D? result = DetectSlope();
        if (result != null) {
            RaycastHit2D slope_hit = result.Value;
            _groundDirection = -Vector2.Perpendicular(slope_hit.normal).normalized;
            Debug.DrawRay(slope_hit.point, slope_hit.normal, Color.green);
            Debug.DrawRay(slope_hit.point, _groundDirection, Color.red);
            if (Mathf.Abs(slope_hit.normal.x) > 1e-4f) { _onSlope = true; }
        }
    }

    protected virtual RaycastHit2D? DetectGround() { return null; }
    protected virtual RaycastHit2D? DetectSlope() { return null; }
    public virtual void SetFriction(string friction_type) {}
    public void SetVelocity(Vector2 velocity) { _rigidbody.velocity = velocity; }
    public Vector2 GetVelocity() { return _rigidbody.velocity; }
    public bool isOnGround() { return _onGround; }
    public bool isOnSlope() { return _onSlope; }
    public Type GetStateType() { return _state.GetType(); }
    public Vector2 GetGroundDirection() { return _groundDirection; }

    public virtual BaseCommand GetCommand(int idx) { return _commandList[idx]; }
    public virtual bool IsContainCommand<Command>() {return _commandList.Any(x => x is Command);}
    public virtual int GetCommandListSize() { return _commandList.Count; }
    public virtual void CleanCommandList() { _commandList.Clear(); }
    public virtual IEnumerable<BaseCommand> GetCommandListEnumable() { return _commandList; }
    public virtual void ReceiveCommands(BaseCommand command) { _commandList.Add(command); }
    public virtual void ExecuteCommand(int idx) { _commandList[idx].Execute(gameObject); }

    public virtual bool ExecuteCommand(Func<BaseCommand, bool> condition)
    {
        var commands = _commandList.Where(condition);
        foreach (var command in commands) {
            command.Execute(gameObject);
        }

        return commands.Count() > 0;
    }
}
}