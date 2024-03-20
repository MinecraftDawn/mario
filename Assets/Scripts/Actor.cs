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
    public float horizontalSpeed = 3f;
    public float jumpForce = 3f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        _state = new OnLandState();
        _commandList = new List<BaseCommand>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        _state = _state.Update(gameObject);
    }

    public virtual void FixedUpdate()
    {
        _state = _state.FixedUpdate(gameObject);
    }

    public virtual bool DetectGround() { return true; }

    public virtual BaseCommand GetCommand(int idx) { return _commandList[idx]; }
    public virtual bool IsContainCommand<Command>() {return _commandList.Any(x => x is Command);}
    public virtual int GetCommandListSize() { return _commandList.Count; }
    public virtual void CleanCommandList() { _commandList.Clear(); }
    public virtual IEnumerable<BaseCommand> GetCommandListEnumable() { return _commandList; }
    public virtual void ReceiveCommands(BaseCommand command) { _commandList.Add(command); }
    public virtual void ExecuteCommand(int idx) { _commandList[idx].Execute(gameObject); }

    public virtual bool ExecuteCommand(Func<BaseCommand, bool> condition) {
        var commands = _commandList.Where(condition);
        foreach (var command in commands) {
            command.Execute(gameObject);
        }

        return commands.Count() > 0;
    }
}
}