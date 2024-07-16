using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Actor;
using JetBrains.Annotations;
using State;
using UnityEngine;

namespace Command
{
public abstract class BaseCommand
{
    public bool isUsing { get; set; }
    public BaseCommand next { get; set; }

    protected void CheckUsing()
    {
        if (isUsing) { return; }
        Debug.Log("[Error] non-using command called Execute.");
    }

    public virtual void Execute(Actor.ActorBase actor) {}

    public override bool Equals(object obj) {
        return GetType().FullName.Equals(obj.GetType().FullName);
    }
    
    public override int GetHashCode() {
        return GetType().FullName.GetHashCode();
    }
}

public class MoveCommand : BaseCommand
{
    protected float _horizontal = 0.0f;

    public MoveCommand() {}
    public MoveCommand(float horizontal) { _horizontal = horizontal; }
    public void SetHorizontal(float horizontal) { _horizontal = horizontal; }
    public override void Execute(Actor.ActorBase actor)
    {
        CheckUsing();
        Vector2 velocity = actor.velocity;
        if (actor.IsStateType<OnLandState>() && actor.IsOnSlope()) {
            velocity = actor.GetGroundDirection() * actor.horizontalSpeed * _horizontal;
        } else {
            velocity.x = actor.horizontalSpeed * _horizontal;
        }
        actor.velocity = velocity;
        Vector2 command_direction = _horizontal > 0.0f ? Vector2.right : Vector2.left;
        if (actor.ObjectFaceDirection() != command_direction) { actor.FlipObject(); }
    }
}

public class SmoothMoveCommand : MoveCommand
{
    public SmoothMoveCommand() {}
    public SmoothMoveCommand(float horizontal) : base(horizontal) {}
    public override void Execute(ActorBase actor)
    {
        CheckUsing();
        Player player = (Player)actor;
        float target_speed = actor.horizontalSpeed * _horizontal;
        float current_speed = player.GetMoveSpeed();
        float lerp = actor.IsStateType<OnLandState>() ? 1.0f : 0.5f;
        target_speed = Mathf.Lerp(current_speed, target_speed, lerp);

        // speed_diff have the direction of the force,
        // compute the force value by speed_diff * accelerate,
        // this method does not have any theory, it is an empirical method.
        float accelerate = player.GetAccelerate();
        float speed_diff = target_speed - current_speed;
        float movement = speed_diff * accelerate;
        player.AddMovementForce(movement);
        Vector2 player_direction = player.velocity.x > 0.0f ? Vector2.right : Vector2.left;
        if (player.ObjectFaceDirection() != player_direction) { player.FlipObject(); }
    }
}

public class JumpCommand : BaseCommand
{
    public JumpCommand() {}
    public override void Execute(Actor.ActorBase actor)
    {
        CheckUsing();
        Vector2 new_velocity = actor.velocity;
        new_velocity.y = 0f;
        actor.velocity = new_velocity;
        actor.GetRigidbody().AddForce(Vector2.up * actor.jumpForce, ForceMode2D.Impulse);
    }
}

public class HoldingJumpCommand : JumpCommand
{
    // Leave execute empty, make this command to be no effect.
    // Used to tracing the button is holding or not.
    public HoldingJumpCommand() {}
    public override void Execute(ActorBase actor)
    {
        CheckUsing();
    }
}

public class TestCommand : BaseCommand {
    public override void Execute(Actor.ActorBase actor)
    {
        Debug.Log("For Test");
    }
}

public class MonsterMoveCommand : MoveCommand
{
    private float speedUp;

    public MonsterMoveCommand() { speedUp = 1f; }
    public MonsterMoveCommand(float speed_up) { speedUp = speed_up; }

    public override void Execute(Actor.ActorBase actor)
    {
        CheckUsing();
        Monster monster = (Monster)actor;
        Vector2 velocity = monster.velocity;
        float direction = monster.GetMoveToRight() ? 1 : -1;
        if (monster.IsOnGround() && monster.IsOnSlope()) {
            velocity = monster.GetGroundDirection() * monster.horizontalSpeed * direction * speedUp;
        } else {
            velocity.x = monster.horizontalSpeed * direction * speedUp;
        }
        monster.velocity = velocity;
    }
}

public class CommandPool
{
    static private bool _isInit = false;
    static private int _poolSize = 20;
    static private Dictionary<string, List<BaseCommand>> _pool;
    static private Dictionary<string, BaseCommand> _validObject;
    static private Dictionary<string, BaseCommand> _sampleObject;

    static public void Register<T>() where T : BaseCommand, new()
    {
        List<BaseCommand> command_pool = new List<BaseCommand>();
        for (int i = 0; i < _poolSize; i++) {
            command_pool.Add(new T());
            if (i != 0) { command_pool[i-1].next = command_pool[i]; }
        }
        command_pool.Last().next = null;
        _validObject.Add(typeof(T).FullName, command_pool[0]);
        _pool.Add(typeof(T).FullName, command_pool);
        _sampleObject.Add(typeof(T).FullName, new T());
    }

    static public void InitPool()
    {
        if (_isInit) { return; }
        _pool = new Dictionary<string, List<BaseCommand>>();
        _validObject = new Dictionary<string, BaseCommand>();
        _sampleObject = new Dictionary<string, BaseCommand>();
        Register<MoveCommand>();
        Register<SmoothMoveCommand>();
        Register<JumpCommand>();
        Register<HoldingJumpCommand>();
        Register<MonsterMoveCommand>();
        _isInit = true;
    }

    public T RequestObject<T>() where T : BaseCommand
    {
        if (!_pool.ContainsKey(typeof(T).FullName)) {
            Debug.Log("[Error] there is no such command type in command pool. type: " + typeof(T).FullName);
            return null;
        }
        T command = (T)_validObject[typeof(T).FullName];
        if (command == null) { Debug.Log("command is null"); }
        if (_validObject[typeof(T).FullName] == null) { Debug.Log("valid object is null"); }
        _validObject[typeof(T).FullName] = command.next;
        command.next = null;
        command.isUsing = true;
        return command;
    }

    public void ReturnObject(BaseCommand obj)
    {
        if (!_pool.ContainsKey(obj.GetType().FullName)) {
            Debug.Log("[Error] there is no such command type in command pool. type: " + obj.GetType().FullName);
            return;
        }
        obj.next = _validObject[obj.GetType().FullName];
        _validObject[obj.GetType().FullName] = obj;
        obj.isUsing = false;
    }

    // Used to debug memory leak issue
    private void CheckNumberValid<T>() where T : BaseCommand
    {
        int counter = 0;
        for (int i = 0; i < _poolSize; i++) {
            if (_pool[typeof(T).FullName][i].isUsing) { continue; }
            counter++;
        }
        Debug.Log("[Debug] Number of valid object of command type " + typeof(T).FullName + ": " + counter);
    }
}

}
