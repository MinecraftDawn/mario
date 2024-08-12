using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils;

namespace Environment
{
namespace Projectile
{

public class Projectile : MonoBehaviour
{
    [SerializeField]
    protected DelayTimer _liveTime;

    void OnEnable()
    {
        _liveTime.UpdateLastTime();
    }

    void Update()
    {
        if (!_liveTime.HasDelayPassed()) { return; }
        gameObject.SetActive(false);
    }
}

}
}
