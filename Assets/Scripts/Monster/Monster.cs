using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;
using Command;
using State;

namespace Monster {

public class Monster : ActorBase
{
    public float frontWallDetectRayLength = 0.1f;
    public Detector detector;
    [SerializeField]
    private float _hitForce = 5f;
    private Strategy.MonsterAI _strategy;
    protected bool _frontWall = false;
    protected bool _moveToRight = true;
    
    /************************************************************
    * Major Actor Flow Method
    ************************************************************/
    public override void Start()
    {
        base.Start();
        _strategy = GetComponent<Strategy.MonsterAI>();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_platformRigidbody == null) { _rigidbody.gravityScale = 1f; }
    }

    protected override void CollectState()
    {
        base.CollectState();
        _frontWall = DetectFrontWall() != null;
    }

    protected override void PreparationBeforeFixedUpdate()
    {
        CollectState();
        if (_strategy != null) { _strategy.Decide(this); }
    }
    
    protected override void InitialState() { _stateManager.Init<MonsterState>(); }

    protected override void FollowPlatform()
    {
        Vector2 temp_velocity = velocity;
        temp_velocity.x += _platformRigidbody.velocity.x;
        velocity = temp_velocity;
        if (IsStateType<MonsterOnLandState>()) {
            _rigidbody.gravityScale = ON_MOVABLE_PLATFORM_GRAVITY;
        }
    }

    /************************************************************
    * Actor Command Method
    ************************************************************/
    
    public override void ReceiveCommands(BaseCommand command)
    {
        if (_commandSet.Contains(command)) {
            _commandPool.ReturnObject(command);
            return;
        }
        base.ReceiveCommands(command);
    }
    
    public override void CleanCommandList()
    {
        foreach (BaseCommand command in _commandSet) {
            _commandPool.ReturnObject(command);
        }
        base.CleanCommandList();
    }
    
    protected override void UpdateCommandHistory()
    {
        foreach (BaseCommand history_command in _commandHistoryInLastCycle) {
            _commandPool.ReturnObject(history_command);
        }
        base.UpdateCommandHistory();
    }

    /************************************************************
    * Monster Specific Method
    ************************************************************/
    
    virtual public void Damaged() { gameObject.SetActive(false); }
    virtual public Vector2 ComputeHitForce(Player player)
    {
        Vector2 to_player = player.transform.position - transform.position;
        Vector2 direction = new Vector2(0.5f, 0.5f).normalized;
        direction.x *= Mathf.Sign(to_player.x);
        return direction * _hitForce;
    }

    virtual protected RaycastHit2D? DetectFrontWall() { return null; }

    public GameObject GetDetectedPlayer() { return detector?.GetDetectedObject(); }
    public bool IsFrontWall() { return _frontWall; }
    public bool GetMoveToRight() { return _moveToRight; }
    public void TurnAround()
    {
        FlipObject();
        _moveToRight = !_moveToRight;
    }
}

}
