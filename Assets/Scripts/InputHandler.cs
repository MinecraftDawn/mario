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
        if (horizontal != 0) {
            SmoothMoveCommand command = player_component.GenerateCommand<SmoothMoveCommand>();
            command.SetHorizontal(horizontal);
            command.SetRunMode(Input.GetKey(KeyCode.LeftShift) ? true : false);
            player_component.ReceiveCommands(command);
        }
        if (Input.GetKey(KeyCode.UpArrow)) {
            if (player_component.IsContainCommandInLastCycle<JumpCommand>()
                || player_component.IsContainCommandInLastCycle<HoldingJumpCommand>()) {
                HoldingJumpCommand command = player_component.GenerateCommand<HoldingJumpCommand>();
                player_component.ReceiveCommands(command);
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                JumpCommand command = player_component.GenerateCommand<JumpCommand>();
                player_component.ReceiveCommands(command);
            }
        }
        
        // For Test
        if(Input.GetKeyDown(KeyCode.Space)) { player_component.ReceiveCommands(new TestCommand()); }
    }
}