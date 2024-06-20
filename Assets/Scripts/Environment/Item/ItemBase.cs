using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    public virtual void Effect(ActorBase actor) { }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }

        ActorBase actor = other.GetComponent<ActorBase>();
        Effect(actor);
    }
}
