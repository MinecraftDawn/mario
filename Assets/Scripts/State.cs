using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Command;
using enums;
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
        // player.ExecuteCommand<MoveCommand>();
        if (player.IsContainCommand<SmoothMoveCommand>()) {
            player.ExecuteCommand<SmoothMoveCommand>();
        } else {
            float current_speed = player.GetMoveSpeed();
            float speed_diff = -current_speed;
            player.AddForce(speed_diff * player.GetDecelerate());
        }
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
            // player.SetFriction(FrictionType.FULL);
        }
        base.FixedUpdate(actor);

        player.ExecuteCommand<TestCommand>(); // Just for test getKeyDown / Up
        
        bool exist_jump = player.ExecuteCommand<JumpCommand>();
        if (exist_jump || !player.IsOnGround()) { return new InAirState(); }

        return this;
    }

    public override void OnStateStart(ActorBase actor) {
        Player player = (Player)actor;
        // player.SetFriction(FrictionType.FULL);
        player.NoDrag();
        // player.SetGravityToBase();
        player.SetGravityToZero();
    }
}

public class InAirState : MovableState{
    // Avoid detecting the ground at the moment of jumping and changing the state to OnLandState
    private int _freezeTick = 1;
    
    public override BaseState FixedUpdate(ActorBase actor)
    {
        base.FixedUpdate(actor);
        Player player = (Player)actor;

        ModifyFallingStatus(player);

        _freezeTick--;
        if (_freezeTick < 0 && player.IsOnGround() && player.IsFalling()) { return new OnLandState(); }
        
        return this;
    }

    public override void OnStateStart(ActorBase actor)
    {
        Player player = (Player)actor;
        player.SetFriction(FrictionType.NONE);
        player.SetGravityToBase();
    }

    private void ModifyFallingStatus(Player player)
    {
        player.ResetDrag();
        if (player.IsFalling()) {
            player.SetGravityToFull();
        } else if (player.ExecuteCommand<HoldingJumpCommand>()) {
            Debug.Log("holding jump");
            // if player holding jump button, let it jump higher
            player.SetGravityToBase();
        } else {
            // if player release jump button early, force it slow down.
            player.SetGravityToHalf();
        }
        
        // clip max y axis velocity
        player.velocity = new Vector2(
            player.velocity.x, Mathf.Max(player.velocity.y, -player.maxFallSpeed));
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
