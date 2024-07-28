using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;
using utils;

namespace State {

public class UnmovableState : BaseState
{
    private DelayTimer duration = new DelayTimer(0f);

    public BaseState Update(ActorBase actor)
    {
        if (duration.HasDelayPassed()) {
            Debug.Log("Can move!");
            if (actor.IsOnGround()) {
                return actor.StateTransition<OnLandState>();
            } else {
                return actor.StateTransition<InAirState>();
            }
        }
        return this;
    }

    public BaseState FixedUpdate(ActorBase actor)
    {
        Player player = (Player)actor;
        if (actor.IsOnGround() && actor.IsOnSlope()) {
            player.SetGravityToZero();
        } else {
            player.SetGravityToFull();
        }
        float current_speed = player.GetMoveSpeed();
        float speed_diff = -current_speed;
        player.AddMovementForce(speed_diff * player.GetDecelerate() / 2);
        return this;
    }

    public BaseState StatusCheck(ActorBase actor) { return this; }

    public void OnStateStart(ActorBase actor)
    {
        Player player = (Player)actor;
        duration.ResetLatency(player.GetUnmoveTime());
        duration.UpdateLastTime();
        player.SetGravityToFull();
        player.SyncMoveSpeedWithVelocityX();
    }

    public void Reset()
    {
        duration.ResetLatency(0); // save in case, maybe no need
    }
}

}
