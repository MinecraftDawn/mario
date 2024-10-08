using Actor;
using Command;

namespace State
{
public class MonsterInAirState : MonsterState
{
    public override BaseState FixedUpdate(ActorBase actor)
    {
        Monster.Monster monster = (Monster.Monster) actor;
        monster.ExecuteCommand<MonsterMoveCommand>();

        if (monster.IsOnGround()) { return monster.StateTransition<MonsterOnLandState>(); }
        return this;
    }
}

}