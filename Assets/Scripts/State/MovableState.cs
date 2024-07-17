using Actor;
using Command;

namespace State
{
public class MovableState : BaseState
{
    public virtual BaseState Update(ActorBase actor) { return this; }
    public virtual BaseState FixedUpdate(ActorBase actor) {
        Player player = (Player) actor;
        // player.ExecuteCommand<MoveCommand>();
        if (player.IsContainCommand<SmoothMoveCommand>()) {
            player.ExecuteCommand<SmoothMoveCommand>();
        } else {
            float current_speed = player.GetMoveSpeed();
            float speed_diff = -current_speed;
            player.AddMovementForce(speed_diff * player.GetDecelerate());
        }
        return this;
    }

    public virtual void OnStateStart(ActorBase actor) { }
}

}