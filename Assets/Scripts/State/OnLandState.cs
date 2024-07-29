using Actor;
using Command;
using enums;

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