using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Command;
using enums;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace State {
public interface BaseState
{
    public BaseState Update(ActorBase actor);
    public BaseState FixedUpdate(ActorBase actor);
    public void OnStateStart(ActorBase actor);
}

public class MovableState : BaseState
{
    public virtual BaseState Update(ActorBase actor) { return this; }
    public virtual BaseState FixedUpdate(ActorBase actor) {
        Player player = (Player) actor;
        player.ExecuteCommand<MoveCommand>();
        return this;
    }

    public virtual void OnStateStart(ActorBase actor) { }
}

public class OnLandState : MovableState
{
    public override BaseState FixedUpdate(ActorBase actor)
    {
        // TODO: change to use parent class Actor.ActorBase
        Player player = (Player) actor;
        if (player.IsContainCommand<MoveCommand>()) {
            player.SetFriction(FrictionType.NONE);
        } else {
            player.SetFriction(FrictionType.FULL);
        }
        base.FixedUpdate(actor);

        player.ExecuteCommand<TestCommand>(); // Just for test getKeyDown / Up
        
        bool exist_jump = player.ExecuteCommand<JumpCommand>();
        if (exist_jump || !player.IsOnGround()) { return new InAirState(); }

        return this;
    }

    public override void OnStateStart(ActorBase actor) {
        Player player = (Player)actor;
        player.SetFriction(FrictionType.FULL);
        player.NoDrag();
        player.SetGravityToBase();
    }
}

public class InAirState : MovableState{
    // Avoid detecting the ground at the moment of jumping and changing the state to OnLandState
    private int _freezeTick = 1;
    
    public override BaseState FixedUpdate(ActorBase actor)
    {
        base.FixedUpdate(actor);
        Player player = (Player)actor;

        bool jump_exist = player.IsContainCommand<JumpCommand>();
        player.SetGravityToBase();
        player.ResetDrag();
        if (player.IsFalling()) {
            player.SetGravityToFull();
        } else if (!jump_exist) {
            player.SetGravityToHalf();
        }
        
        Vector2 velocity = player.velocity;
        velocity.y = Mathf.Max(velocity.y, -player.maxFallSpeed);
        player.velocity = velocity;

        _freezeTick--;
        if (_freezeTick < 0 && player.IsOnGround() && player.IsFalling()) { return new OnLandState(); }
        
        return this;
    }

    public override void OnStateStart(ActorBase actor)
    {
        Player player = (Player)actor;
        player.SetFriction(FrictionType.NONE);
    }
}

public class MonsterState : BaseState
{
    public virtual BaseState Update(ActorBase actor) { return this; }
    public virtual BaseState FixedUpdate(ActorBase actor) { return this; }
    public virtual void OnStateStart(ActorBase actor) {}
}

public class MonsterOnLandState : MonsterState
{
    public override BaseState FixedUpdate(ActorBase actor) {
        Monster monster = (Monster) actor;
        bool x = monster.ExecuteCommand<MoveCommand>();

        bool exist_jump = monster.ExecuteCommand<JumpCommand>();
        if (exist_jump || !monster.IsOnGround()) { return new MonsterInAirState(); }

        return this;
    }
}

public class MonsterInAirState : MonsterState
{
    public override BaseState FixedUpdate(ActorBase actor)
    {
        Monster monster = (Monster) actor;
        monster.ExecuteCommand<MoveCommand>();

        if (monster.IsOnGround()) { return new MonsterOnLandState(); }
        return this;
    }
}

}
