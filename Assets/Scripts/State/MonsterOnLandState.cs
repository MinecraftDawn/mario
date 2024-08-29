using Actor;
using Command;

namespace State
{
public class MonsterOnLandState : MonsterState
{
    public override BaseState FixedUpdate(ActorBase actor) {
        Monster.Monster monster = (Monster.Monster) actor;
        bool x = monster.ExecuteCommand<MonsterMoveCommand>();

        bool exist_jump = monster.ExecuteCommand<JumpCommand>();
        if (exist_jump || !monster.IsOnGround()) { return monster.StateTransition<MonsterInAirState>(); }

        return this;
    }
}

}