using System;
using UnityEngine;

namespace utils
{
    
[Serializable]
public class DelayTimer {
    [SerializeField]
    protected float _delay;
    protected float _lastTime;
    
    public DelayTimer() : this(1f) {}

    public DelayTimer(float delay) {
        _delay = delay;
    }
    
    public bool HasDelayPassed()
    {
        return Time.time - _lastTime > _delay;
    }
    
    public void UpdateLastTime()
    {
        _lastTime = Time.time;
    }

    // Not currently used
    public float GetRemainingTime() {
        return Mathf.Max(0, Time.time - _lastTime);
    }

    // Not currently used
    public void ResetLatency(float delay) {
        _delay = delay;
    }

    private float Adjustlatency(float delay) {
        if (delay < 0) { Debug.LogWarning("Set Latency small than 0"); }

        return Mathf.Max(0, delay);
    }
}
}