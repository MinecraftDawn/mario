using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MonsterHead") {
            Debug.Log("Monter enter");
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
