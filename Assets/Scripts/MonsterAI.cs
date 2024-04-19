using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy {

public interface MonsterAI
{
    public BaseCommand Decide(Monster monster);
}

public class KeepMove : MonsterAI
{
    public BaseCommand Decide(Monster monster) 
    {
        if (monster.IsFrontWall()) { monster.TurnAround(); }
        return new MonsterMoveCommand();
    }
}

}
