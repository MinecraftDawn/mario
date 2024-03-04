using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Command
{
public interface BaseCommand
{
    public void Execute(GameObject actor);
}

public class PlayerCommand : BaseCommand
{
    public virtual void Execute(GameObject actor) {}
}

public class MoveCommand : PlayerCommand
{
    private float _horizontal = 0.0f;

    public MoveCommand(float horizontal) { _horizontal = horizontal;  }
    public override void Execute(GameObject actor)
    {
        // TODO: the player's move command
    }
}

public class JumpCommand : PlayerCommand
{
    public override void Execute(GameObject actor)
    {
        // TODO: the player's jump command
    }
}
}
