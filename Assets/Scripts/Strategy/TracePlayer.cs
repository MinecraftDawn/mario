using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Command;

namespace Strategy
{

public class TracePlayer : KeepMove
{
    public float speedUpWhenTracing = 1.5f;
    public float playerDetectTime = 3f;
    public float tracePlayerTurnAroundDelay = 0.8f;
    public Detector playerDetector;
    private GameObject _player;
    private float _lastPlayerDetectTime;
    private float _lastTracePlayerTurnAroundTime;

    public override void Decide(Monster monster)
    {
        UpdatePlayerDetection();
        if (IsPlayerDetected()) {
            Tracing(monster);
        }
        else {
            base.Decide(monster);
        }
    }
    
    private void UpdatePlayerDetection() {
        if (playerDetector.IsDetected()) {
            _player = playerDetector.GetDetectedObject();
            UpdateLastDetectPlayerTime();
        }

        if (IsForgetPlayer()) { _player = null; }
    }
    
    private bool IsPlayerDetected()
    {
        return _player != null;
    }

    private void Tracing(Monster monster)
    {
        Vector2 monster_to_player = 
            _player.transform.position - monster.transform.position;
        if ((monster.GetMoveToRight() && monster_to_player.x < 0f) ||
            (!monster.GetMoveToRight() && monster_to_player.x > 0f)) {
            if (CanTurnAroundToTrackPlayer()) {
                monster.TurnAround();
                UpdateLastTurnAroundTime();
            }
        }
        if (monster.IsFrontWall()) { monster.ReceiveCommands(new JumpCommand()); }
        monster.ReceiveCommands(new MonsterMoveCommand(speedUpWhenTracing));
    }
    
    private bool CanTurnAroundToTrackPlayer()
    {
        return Time.time - _lastTracePlayerTurnAroundTime > tracePlayerTurnAroundDelay;
    }
    
    private void UpdateLastTurnAroundTime()
    {
        _lastTracePlayerTurnAroundTime = Time.time;
    }
    
    private bool IsForgetPlayer()
    {
        return Time.time - _lastPlayerDetectTime > playerDetectTime;
    }
    
    private void UpdateLastDetectPlayerTime()
    {
        _lastPlayerDetectTime = Time.time;
    }
}

}
