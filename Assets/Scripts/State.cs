using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Command;
using Unity.VisualScripting;
using UnityEngine;

namespace State {
public interface BaseState
{
    public BaseState Update(GameObject actor);
    public BaseState FixedUpdate(GameObject actor);
}

public class MovableState : BaseState
{
    public virtual BaseState Update(GameObject actor) { return this; }
    public virtual BaseState FixedUpdate(GameObject actor) {
        Player player = actor.GetComponent<Player>();
        player.ExecuteCommand(x => x is MoveCommand);
        return this;
    }
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
        if (existJump) { return new InAirState(); }

        return this;
    }
}

public class InAirState : MovableState
{ 
    public override BaseState FixedUpdate(GameObject actor)
    {
        base.FixedUpdate(actor);
        Actor.ActorBase agent = actor.GetComponent<Actor.ActorBase>();
        if (!agent.DetectGround()) { return this; }
        
        agent.SetFriction("full");
        return new OnLandState();
    }
    
    private bool isFalling(Rigidbody2D rigidbody) { return rigidbody.velocity.y < 0.0f; }
}

}
