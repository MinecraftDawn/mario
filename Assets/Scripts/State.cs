using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Command;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace State {
public interface BaseState
{
    public BaseState Update(GameObject actor);
    public BaseState FixedUpdate(GameObject actor);
    public void OnStateStart(GameObject actor);
}

public class MovableState : BaseState
{
    public virtual BaseState Update(GameObject actor) { return this; }
    public virtual BaseState FixedUpdate(GameObject actor) {
        Player player = actor.GetComponent<Player>();
        player.ExecuteCommand(x => x is MoveCommand);
        return this;
    }

    public virtual void OnStateStart(GameObject actor) { }
}

public class OnLandState : MovableState
{
    public override BaseState FixedUpdate(GameObject actor)
    {
        // TODO: change to use parent class Actor.ActorBase
        Player player = actor.GetComponent<Player>();
        if (player.IsContainCommand<MoveCommand>()) {
            player.SetFriction("none");
        } else {
            player.SetFriction("full");
        }
        base.FixedUpdate(actor);
        
        bool existJump = player.ExecuteCommand(x => x is JumpCommand);
        if (existJump || !player.IsOnGround()) { return new InAirState(); }

        return this;
    }

    public override void OnStateStart(GameObject actor) {
        Actor.ActorBase agent = actor.GetComponent<Actor.ActorBase>();
        agent.SetFriction("full");
    }
}

public class InAirState : MovableState
{ 
    public override BaseState FixedUpdate(GameObject actor)
    {
        base.FixedUpdate(actor);
        Actor.ActorBase agent = actor.GetComponent<Actor.ActorBase>();
        if (!agent.IsOnGround()) { return this; }
        
        return new OnLandState();
    }

    public override void OnStateStart(GameObject actor)
    {
        Actor.ActorBase agent = actor.GetComponent<Actor.ActorBase>();
        agent.SetFriction("none");
    }

    private bool isFalling(Rigidbody2D rigidbody) { return rigidbody.velocity.y < 0.0f; }
}

public class MonsterState : BaseState
{
    public virtual BaseState Update(GameObject actor) { return this; }
    public virtual BaseState FixedUpdate(GameObject actor) { return this; }
    public virtual void OnStateStart(GameObject actor) {}
}

public class MonsterOnLandState : MonsterState
{
    public override BaseState FixedUpdate(GameObject actor) {
        Monster monster = actor.GetComponent<Monster>();
        monster.ExecuteCommand(x => x is MonsterMoveCommand);
        return this;
    }
}

}
