using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    [SerializeField]
    private float _reactionForce = 5f;
    private Player _player;

    void Start()
    {
        _player = transform.parent.GetComponent<Player>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "MonsterHead") {
            Vector2 velocity = _player.velocity;
            velocity.y = 0f;
            _player.velocity = velocity;
            _player.GetRigidbody().AddForce(_reactionForce * Vector2.up, ForceMode2D.Impulse);
            Destroy(other.gameObject.transform.parent.gameObject);
        }
    }
}
