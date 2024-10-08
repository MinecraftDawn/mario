using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy
{

public class KeepMove : MonsterAI
{
    public override void Decide(Monster.Monster monster) 
    {
        if (monster.IsFrontWall() || !monster.IsFrontFloor()) { monster.TurnAround(); }

        MonsterMoveCommand command = monster.GenerateCommand<MonsterMoveCommand>();
        monster.ReceiveCommands(command);
    }
}

}
