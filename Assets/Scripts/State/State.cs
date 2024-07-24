using Actor;

namespace State {
public interface BaseState
{
    public BaseState Update(ActorBase actor);
    public BaseState FixedUpdate(ActorBase actor);
    public BaseState StatusCheck(ActorBase actor);
    public void OnStateStart(ActorBase actor);
    public void Reset();
}

}
