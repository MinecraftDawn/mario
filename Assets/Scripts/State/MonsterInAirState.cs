using Actor;
using Command;

namespace State
{
public class MonsterInAirState : MonsterState
{
    public override BaseState FixedUpdate(ActorBase actor)
    {
        Monster monster = (Monster) actor;
        monster.ExecuteCommand<MoveCommand>();

        if (monster.IsOnGround()) { return new MonsterOnLandState(); }
        return this;
    }
}

}