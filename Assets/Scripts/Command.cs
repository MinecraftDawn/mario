using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Command
{
public interface BaseCommand
{
    public void Execute(GameObject gameObject);
}

public class PlayerCommand : BaseCommand
{
    public virtual void Execute(GameObject gameObject) {}
}

public class MoveCommand : PlayerCommand
{
    private float _horizontal = 0.0f;

    public MoveCommand(float horizontal) { _horizontal = horizontal;  }
    public override void Execute(GameObject gameObject)
    {
        // TODO: the player's move command
        Player player = gameObject.GetComponent<Player>();
        Vector2 velocity = player.GetVelocity();
        velocity.x = player.horizontalSpeed * _horizontal;
        player.SetVelocity(velocity);
    }
}

public class JumpCommand : PlayerCommand
{
    public override void Execute(GameObject gameObject)
    {
        // Debug.Log("jump is executed.");
        Rigidbody2D rigidbody = gameObject.GetComponent<Rigidbody2D>();
        Player player = gameObject.GetComponent<Player>();
        rigidbody.AddForce(Vector2.up * player.jumpForce, ForceMode2D.Impulse); 
    }
}
}
