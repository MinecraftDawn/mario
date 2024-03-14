using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Command;
using UnityEngine;

namespace State {
public interface BaseState
{
    public BaseState Update(GameObject actor);
    public BaseState FixedUpdate(GameObject actor);
}

// TODO: Maybe you will need a Movable state to prevent some redundant code.

public class MovableState : BaseState
{
    public virtual BaseState Update(GameObject actor) {
        return this;
    }

    public virtual BaseState FixedUpdate(GameObject actor) {
        Player player = actor.GetComponent<Player>();
        Vector2 ori_position = actor.GetComponent<Rigidbody2D>().position;
        
        player.ExecuteCommand(x => x is MoveCommand);
        
        return this;
    }
}

public class OnLandState : MovableState
{
    public override BaseState FixedUpdate(GameObject actor)
    {
        // TODO: need to fill in
        base.FixedUpdate(actor);
        
        Player player = actor.GetComponent<Player>();
        
        // for (int i = 0; i < player.GetCommandListSize(); i++) {
        //     BaseCommand command = player.GetCommand(i);
        //     if (command is JumpCommand) {
        //         player.ExecuteCommand(i);
        //         return new InAirState();
        //     }
        // }
        bool existJump = player.ExecuteCommand(x => x is JumpCommand);
        if (existJump) {  return new InAirState();}
        

        return this;
    }
}

public class InAirState : MovableState
{ 
    public override BaseState FixedUpdate(GameObject actor)
    {
        base.FixedUpdate(actor);
        Rigidbody2D rigidbody = actor.GetComponent<Rigidbody2D>();
        if (!isOnGround(rigidbody)) { return this; }
        
        return new OnLandState();
    }
    
    private bool isFalling(Rigidbody2D rigidbody) { return rigidbody.velocity.y < 0.0f; }
    
    
    private bool isOnGround(Rigidbody2D rigidbody) {
        LayerMask mask = LayerMask.GetMask("Ground");
        RaycastHit2D hit = Physics2D.Raycast(rigidbody.position, -Vector2.up, 0.05f, mask);
        return hit.collider != null;
    }
}

}
