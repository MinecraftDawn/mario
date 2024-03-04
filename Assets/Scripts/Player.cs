using System.Collections;
using System.Collections.Generic;
using State;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BaseState _state;

    // Start is called before the first frame update
    void Start()
    {
        _state = new OnLandState();
    }

    // Update is called once per frame
    void Update()
    {
        _state = _state.Update(gameObject);
    }

    void FixedUpdate()
    {
        _state = _state.FixedUpdate(gameObject);
    }
}
