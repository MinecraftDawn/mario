using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy
{

public class TracePlayer : KeepMove
{
    public float speedUpWhenTracing = 1.5f;

    public override void Decide(Monster monster)
    {
        if (!monster.IsPlayerFound()) {
            base.Decide(monster);
        } else {
            // trace player
            Tracing(monster);
        }
    }

    private void Tracing(Monster monster)
    {
        Vector2 monster_to_player = 
            monster.GetPlayer().transform.position - monster.transform.position;
        if ((monster.GetMoveToRight() && monster_to_player.x < 0f) ||
            (!monster.GetMoveToRight() && monster_to_player.x > 0f)) {
            monster.TurnAround();
        }
        if (monster.IsFrontWall()) { monster.ReceiveCommands(new JumpCommand()); }
        monster.ReceiveCommands(new MonsterMoveCommand(speedUpWhenTracing));
    }
}

}
