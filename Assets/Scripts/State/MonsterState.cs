using Actor;

namespace State
{
public class MonsterState : BaseState
{
    public virtual BaseState Update(ActorBase actor) { return this; }
    public virtual BaseState FixedUpdate(ActorBase actor) { return this; }
    public virtual BaseState StatusCheck(ActorBase actor) { return this; }
    public virtual void OnStateStart(ActorBase actor) {}
    public virtual void Reset() {}
}

}