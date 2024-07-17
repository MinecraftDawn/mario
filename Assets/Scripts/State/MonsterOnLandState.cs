using Actor;
using Command;

namespace State
{
public class MonsterOnLandState : MonsterState
{
    public override BaseState FixedUpdate(ActorBase actor) {
        Monster monster = (Monster) actor;
        bool x = monster.ExecuteCommand<MoveCommand>();

        bool exist_jump = monster.ExecuteCommand<JumpCommand>();
        if (exist_jump || !monster.IsOnGround()) { return new MonsterInAirState(); }

        return this;
    }
}

}