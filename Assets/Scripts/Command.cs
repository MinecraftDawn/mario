using System;
using System.Collections;
using System.Collections.Generic;
using State;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Command
{
public abstract class BaseCommand
{
    public virtual void Execute(GameObject gameObject) { }

    public override bool Equals(object obj) {
        return GetType().FullName.Equals(obj.GetType().FullName);
    }
    
    public override int GetHashCode() {
        return GetType().FullName.GetHashCode();
    }
}

public class PlayerCommand : BaseCommand
{
    public override void Execute(GameObject gameObject) {}
}

public class MoveCommand : PlayerCommand
{
    private float _horizontal = 0.0f;

    public MoveCommand(float horizontal) { _horizontal = horizontal;  }
    public override void Execute(GameObject gameObject)
    {
        // TODO: the player's move command
        Player player = gameObject.GetComponent<Player>();
        Vector2 velocity = player.velocity;
        if (player.IsStateType<OnLandState>() && player.IsOnSlope()) {
            velocity = player.GetGroundDirection() * player.horizontalSpeed * _horizontal;
        } else {
            velocity.x = player.horizontalSpeed * _horizontal;
        }
        player.velocity = velocity;
    }
}

public class JumpCommand : PlayerCommand
{
    public override void Execute(GameObject gameObject)
    {
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Actor.ActorBase actor = gameObject.GetComponent<Actor.ActorBase>();
        Vector2 new_velocity = rigidbody.velocity;
        new_velocity.y = 0f;
        rigidbody.velocity = new_velocity;
        rigidbody.AddForce(Vector2.up * actor.jumpForce, ForceMode2D.Impulse); 
    }
}

public class TestCommand : PlayerCommand {
    public override void Execute(GameObject gameObject)
    {
        Debug.Log("For Test");
    }
}

public class MonsterCommand : BaseCommand
{
    public override void Execute(GameObject gameObject) {}
}

public class MonsterMoveCommand : MonsterCommand
{
    private float speedUp;

    public MonsterMoveCommand() { speedUp = 1f; }
    public MonsterMoveCommand(float speed_up) { speedUp = speed_up; }

    // Need refactor: the argument of Execute should be Actor
    public override void Execute(GameObject gameObject)
    {
        Monster monster = gameObject.GetComponent<Monster>();
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
