using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{

public class PlayerDead : EventBase
{
    public void UpdateContext(GameContext context)
    {
        Debug.Log("[Debug] Event: Player dead");
        context.GameOver();
    }
}

}
