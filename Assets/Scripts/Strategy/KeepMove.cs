using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy
{

public class KeepMove : MonsterAI
{
    public override void Decide(Monster monster) 
    {
        if (monster.IsFrontWall()) { monster.TurnAround(); }
        monster.ReceiveCommands(new MonsterMoveCommand());
    }
}

}
