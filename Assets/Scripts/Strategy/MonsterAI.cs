using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy
{

public abstract class MonsterAI : MonoBehaviour
{
    public virtual void Decide(Monster monster) {}
}

}
