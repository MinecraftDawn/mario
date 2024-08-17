using UnityEngine;
using Actor;
using Command;
using Monster;

namespace State
{

public class FortActiveState : BaseState
{
    public BaseState Update(ActorBase actor) { return this; }
    public BaseState FixedUpdate(ActorBase actor)
    {
        Monster.Monster monster = (Monster.Monster)actor;
        if (monster.IsContainCommand<FireProjectileCommand>()) {
            monster.ExecuteCommand<FireProjectileCommand>();
        }
        return this;
    }

    public BaseState StatusCheck(ActorBase actor) { return this; }
    public void OnStateStart(ActorBase actor)
    {
        MagicFort fort = (MagicFort)actor;
        fort.SetToActiveColor();
        fort.ActiveWeakness();
    }
    public void Reset() {}
}

}
