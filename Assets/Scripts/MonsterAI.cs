using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy {

public interface MonsterAI
{
    public void Decide(Monster monster);
}

public class KeepMove : MonsterAI
{
    public void Decide(Monster monster) 
    {
        if (monster.IsFrontWall()) { monster.TurnAround(); }
        monster.ReceiveCommands(new MonsterMoveCommand());
    }
}

}
