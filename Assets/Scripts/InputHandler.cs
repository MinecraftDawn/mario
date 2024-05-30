using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

class InputHandler : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        // TODO: prevent to reallocate the command, this may affect FPS.
        if (horizontal != 0) { player.GetComponent<Player>().ReceiveCommands(new MoveCommand(horizontal)); }
        if (Input.GetKey(KeyCode.UpArrow)) { player.GetComponent<Player>().ReceiveCommands(new JumpCommand()); }
        
        // For Test
        if(Input.GetKeyDown(KeyCode.Space)) { player.GetComponent<Player>().ReceiveCommands(new TestCommand()); }
    }
}