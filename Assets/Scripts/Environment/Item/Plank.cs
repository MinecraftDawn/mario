using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;
using utils;

public class Plank : ItemBase
{
    [SerializeField]
    private float _triggerCoolDown = 3f;
    [SerializeField]
    private DelayTimer _triggerCoolDownTimer;
    private bool _canTrigger;

    void Start()
    {
        _triggerCoolDownTimer = new DelayTimer(_triggerCoolDown);
        _triggerCoolDownTimer.UpdateLastTime();
        _canTrigger = true;
    }

    void Update()
    {
        if (!_canTrigger && _triggerCoolDownTimer.HasDelayPassed()) { _canTrigger = true; }
    }

    public override void Effect(ActorBase actor)
    {
        if (_canTrigger) { 
            Debug.Log("Trigger the plank trap");
            _canTrigger = false;
            _triggerCoolDownTimer.UpdateLastTime();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        ActorBase actor = other.GetComponent<ActorBase>();
        Effect(actor);
    }
}
