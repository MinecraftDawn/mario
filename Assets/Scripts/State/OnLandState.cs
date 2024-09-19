using Actor;
using Command;
using enums;
using UnityEngine;

namespace State
{
public class OnLandState : MovableState
{
    public override BaseState FixedUpdate(ActorBase actor)
    {
        Player player = (Player) actor;
        if (!player.IsContainCommand<SmoothMoveCommand>() && player.IsOnSlope()) {
            // prevent player slide fown from slope
            player.SetGravityToZero();
        } else {
            player.SetGravityToBase();
        }

        if (Mathf.Abs(player._moveSpeed) < 0.05) {
            player.animator.SetBool("idle", true);

        }
        else {
            player.animator.SetBool("idle", false);

        }
        
        
        base.FixedUpdate(actor);

        // player.ExecuteCommand<TestCommand>(); // Just for test getKeyDown / Up
        
        bool exist_jump = player.ExecuteCommand<JumpCommand>();
        if (exist_jump || !player.IsOnGround()) { return player.StateTransition<InAirState>(); }
        
        return this;
    }

    public override void OnStateStart(ActorBase actor) {
        Player player = (Player)actor;
        player.SetFriction(FrictionType.NONE);
        player.NoDrag();
        player.SetGravityToBase();
    }
}

}