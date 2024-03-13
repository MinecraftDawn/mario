using System;
using System.Collections;
using System.Collections.Generic;
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
        
        for (int i = 0; i < player.GetCommandListSize(); i++) {
            BaseCommand command = player.GetCommand(i);
            if (command is MoveCommand) { player.ExecuteCommand(i); }
        }
        

        return this;
    }
}

public class OnLandState : MovableState
{
    // public override BaseState Update(GameObject actor)
    // {
    //     // TODO: need to fill in
    //     return this;
    // }

    public override BaseState FixedUpdate(GameObject actor)
    {
        // TODO: need to fill in
        base.FixedUpdate(actor);

        return this;
    }
}

public class InAirState : MovableState
{
    // public override BaseState Update(GameObject actor)
    // {
    //     // TODO: need to fill in
    //     return this;
    // }

    public override BaseState FixedUpdate(GameObject actor)
    {
        // TODO: need to fill in
        return this;
    }
}

}
