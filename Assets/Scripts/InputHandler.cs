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
        Player player_component = player.GetComponent<Player>();
        if (Input.GetKey(KeyCode.Escape)) { Application.Quit(); }
        // TODO: prevent to reallocate the command, this may affect FPS.
        // if (horizontal != 0) { player_component.ReceiveCommands(new MoveCommand(horizontal)); }
        if (horizontal != 0) { player_component.ReceiveCommands(new SmoothMoveCommand(horizontal)); }
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (player_component.IsContainCommandInLastCycle<JumpCommand>()
                || player_component.IsContainCommandInLastCycle<HoldingJumpCommand>()) {
                player_component.ReceiveCommands(new HoldingJumpCommand());
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                player_component.ReceiveCommands(new JumpCommand());
            }
        }
        
        // For Test
        if(Input.GetKeyDown(KeyCode.Space)) { player_component.ReceiveCommands(new TestCommand()); }
    }
}