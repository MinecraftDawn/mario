using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public class Teleport : ItemBase
{
    [SerializeField]
    private Teleport _target;
    private bool _isAvailable;

    void Start() { _isAvailable = true; }

    public override void Effect(ActorBase actor)
    {
        if (!_isAvailable) {
            _isAvailable = true;
            return;
        }
        Debug.Log("player should be teleport");
        _target.SetIgnoreOnce();
        actor.transform.position = _target.transform.position;
    }

    public void SetIgnoreOnce() { _isAvailable = false; }
}
