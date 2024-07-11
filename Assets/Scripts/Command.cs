using System;
using System.Collections;
using System.Collections.Generic;
using Actor;
using State;
using UnityEngine;

namespace Command
{
public abstract class BaseCommand
{
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
    public override void Execute(Actor.ActorBase actor)
    {
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
    public override void Execute(Actor.ActorBase actor)
    {
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
    public override void Execute(ActorBase actor) {}
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

}
