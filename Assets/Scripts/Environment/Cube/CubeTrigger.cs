using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    [SerializeField]
    private Sprite _noEffectSprite;
    [SerializeField]
    private GameObject _hiddingObject;
    private SpriteRenderer _parentRenderer;
    private bool _triggered = false;

    void Start()
    {
        _parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        if (_hiddingObject != null) { _hiddingObject.SetActive(false); }
        if (_parentRenderer == null) {
            Debug.Log("[Warning] The parent of CubeTrigger have no SpriteRenderer");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_triggered) { return; }
        if (other.gameObject.tag == "Player") {
            Debug.Log("[Debug] player hit cube");
            _parentRenderer.sprite = _noEffectSprite;
            if (_hiddingObject != null) { _hiddingObject.SetActive(true); }
            _triggered = true;
        }
    }
}
