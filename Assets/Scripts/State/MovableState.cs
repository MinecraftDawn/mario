using Actor;
using Command;
using UnityEngine;

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
            float target_speed = 0f;
            float lerp = actor.IsOnGround() ? 1.0f : 0.5f;
            target_speed = Mathf.Lerp(current_speed, target_speed, lerp);
            float speed_diff = target_speed - current_speed;
            player.AddMovementForce(speed_diff * player.GetDecelerate());
        }
        return this;
    }

    public virtual BaseState StatusCheck(ActorBase actor) { return this; }

    public virtual void OnStateStart(ActorBase actor) { }
    public virtual void Reset() {}
}

}