using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;
using utils;

#nullable enable

namespace Strategy
{

public class TracePlayer : KeepMove
{
    public float speedUpWhenTracing = 1.5f;
    [SerializeField]
    public DelayTimer playerDetectTimer = new DelayTimer(3f);
    [SerializeField]
    public DelayTimer tracePlayerTurnAroundTimer = new DelayTimer(0.8f);
    private GameObject? _player;

    public override void Decide(Monster monster)
    {
        UpdatePlayerDetection(monster);
        if (IsPlayerDetected()) {
            Tracing(monster);
        }
        else {
            base.Decide(monster);
        }
    }
    
    #nullable enable
    private void UpdatePlayerDetection(Monster monster) {
        GameObject? player = monster.GetDetectedPlayer();
        if (player is not null) {
            _player = player;
            playerDetectTimer.UpdateLastTime();
        }
        else if(playerDetectTimer.HasDelayPassed()) { _player = null; }
    }
    
    private bool IsPlayerDetected()
    {
        return _player != null;
    }

    private void Tracing(Monster monster) {
        if (_player is null) return;
        
        Vector2 monster_to_player = 
            _player.transform.position - monster.transform.position;
        if ((monster.GetMoveToRight() && monster_to_player.x < 0f) ||
            (!monster.GetMoveToRight() && monster_to_player.x > 0f)) {
            if (tracePlayerTurnAroundTimer.HasDelayPassed()) {
                monster.TurnAround();
                tracePlayerTurnAroundTimer.UpdateLastTime();
            }
        }
        if (monster.IsFrontWall()) { monster.ReceiveCommands(new JumpCommand()); }
        monster.ReceiveCommands(new MonsterMoveCommand(speedUpWhenTracing));
    }
}

}
