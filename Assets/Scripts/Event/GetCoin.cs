using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Event
{

public class GetCoin : EventBase
{
    private int _coinValue { get; }
    public GetCoin(int coin_value) { _coinValue = coin_value; }
    public void UpdateContext(GameContext context)
    {
        Debug.Log("GetCoin Update Game State! + " + _coinValue + " score");
        context.AddScore(_coinValue);
    }
}

}
