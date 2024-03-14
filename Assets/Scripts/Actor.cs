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
    protected List<BaseCommand> _command_list;
    public float horizontal_speed = 3f;
    public float jump_force = 3f;    

    // Start is called before the first frame update
    public void Start()
    {
        _state = new OnLandState();
        _command_list = new List<BaseCommand>();
    }

    // Update is called once per frame
    public void Update()
    {
        _state = _state.Update(gameObject);
    }

    public void FixedUpdate()
    {
        _state = _state.FixedUpdate(gameObject);
    }

    public virtual BaseCommand GetCommand(int idx) { return _command_list[idx]; }
    public virtual bool isContainCommand<Command>() {return _command_list.Any(x => x is Command);}
    public virtual int GetCommandListSize() { return _command_list.Count; }
    public virtual void CleanCommandList() { _command_list.Clear(); }
    public virtual IEnumerator GetCommandListEnumerator() { return _command_list.GetEnumerator(); }
    public virtual IEnumerable<BaseCommand> GetCommandListEnumable() { return _command_list; }
    public virtual void ReceiveCommands(BaseCommand command) { _command_list.Add(command); }
    public virtual void ExecuteCommand(int idx) { _command_list[idx].Execute(gameObject); }

    public virtual bool ExecuteCommand(Func<BaseCommand, bool> condition) {
        var commands = _command_list.Where(condition);
        foreach (var command in commands) {
            command.Execute(gameObject);
        }

        return commands.Count() > 0;
    }
}
}