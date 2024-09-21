using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{

public class GameFinish : EventBase
{
    public void UpdateContext(GameContext context)
    {
        Debug.Log("[Debug] Event: Game finished");
        context.GameFinish();
    }
}

}
