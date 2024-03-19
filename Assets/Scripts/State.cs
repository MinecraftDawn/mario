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
        bool existJump = player.ExecuteCommand(x => x is JumpCommand);
        if (existJump) { 
            Debug.Log("Switch to in air state.");
            return new InAirState();
        }

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
        
        Debug.Log("Switch to on land state");
        return new OnLandState();
    }
    
    private bool isFalling(Rigidbody2D rigidbody) { return rigidbody.velocity.y < 0.0f; }
    
    
    private bool isOnGround(Rigidbody2D rigidbody) {
        LayerMask mask = LayerMask.GetMask("Ground");
        bool result = Physics2D.OverlapCircle(rigidbody.position + new Vector2(0, 0.3f), 0.31f, mask);
        return result && Mathf.Abs(rigidbody.velocity.y) < 1e-4f;
    }
}

}
