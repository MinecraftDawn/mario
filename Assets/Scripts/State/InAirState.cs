using UnityEngine;
using Actor;
using Command;
using enums;

namespace State
{
public class InAirState : MovableState{
    // Avoid detecting the ground at the moment of jumping and changing the state to OnLandState
    private int _freezeTick = 1;
    private bool _startFalling = false;
    
    public override BaseState FixedUpdate(ActorBase actor)
    {
        Player player = (Player)actor;
        _startFalling = _startFalling || player.IsFalling();
        base.FixedUpdate(actor);
        ModifyFallingStatus(player);
        return this;
    }

    public override BaseState StatusCheck(ActorBase actor)
    {
        _freezeTick--;
        if (_freezeTick < 0 && actor.IsOnGround()) { return new OnLandState(); }
        return this;
    }

    public override void OnStateStart(ActorBase actor)
    {
        Player player = (Player)actor;
        player.SetFriction(FrictionType.NONE);
        player.SetGravityToBase();
    }

    private void ModifyFallingStatus(Player player)
    {
        player.ResetDrag();
        if (_startFalling) {
            player.SetGravityToFull();
        } else if (player.ExecuteCommand<HoldingJumpCommand>()) {
            // if player holding jump button, let it jump higher
            player.SetGravityToBase();
        } else {
            // if player release jump button early, force it slow down.
            player.SetGravityToHalf();
        }
        
        // clip max y axis velocity
        player.velocity = new Vector2(
            player.velocity.x, Mathf.Max(player.velocity.y, -player.maxFallSpeed));
    }
}
}