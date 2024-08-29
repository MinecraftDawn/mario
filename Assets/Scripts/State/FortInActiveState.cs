using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;
using utils;
using Monster;

namespace State
{

public class FortInActiveState : BaseState
{
    private DelayTimer timer;
    public FortInActiveState() { timer = new DelayTimer(); }
    public BaseState Update(ActorBase actor) { return this; }
    public BaseState FixedUpdate(ActorBase actor)
    {
        if (timer.HasDelayPassed()) { return actor.StateTransition<FortActiveState>(); }
        return this;
    }
    public BaseState StatusCheck(ActorBase actor) { return this; }
    public void OnStateStart(ActorBase actor)
    {
        MagicFort fort = (MagicFort)actor;
        timer.ResetLatency(fort.GetInactiveLatency());
        timer.UpdateLastTime();
        fort.SetToInactiveColor();
        fort.InActiveWeakness();
        Debug.Log("on state start");
    }
    public void Reset() {}
}

}