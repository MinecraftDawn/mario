using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;
using Command;
using State;

public class Monster : ActorBase
{
    public float frontWallDetectRayLength = 0.1f;
    public Detector detector;
    [SerializeField]
    private float _hitForce = 5f;
    private Strategy.MonsterAI _strategy;
    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _capsuleSize;
    protected bool _frontWall;
    protected bool _moveToRight = true;
    
    // Start is called before the first frame update
    public override void Start()
    {
        // Need refactor
        base.Start();
        _strategy = GetComponent<Strategy.MonsterAI>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleSize = _capsuleCollider == null ? Vector2.zero : _capsuleCollider.size;
    }

    protected override void PreparationBeforeFixedUpdate()
    {
        CollectState();
        if (_strategy != null) { _strategy.Decide(this); }
    }
    
    public GameObject GetDetectedPlayer() {
        return detector?.GetDetectedObject();
    }

    protected override void CollectState()
    {
        base.CollectState();
        _frontWall = DetectFrontWall() != null;
    }

    protected RaycastHit2D? DetectFrontWall()
    {
        Vector2 start_position = _rigidbody.position;
        Vector2 direction = Vector2.right * (GetMoveToRight() ? 1 : -1);
        start_position.y += _capsuleSize.y / 2;
        float ray_length = _capsuleSize.x / 2 + frontWallDetectRayLength;
        RaycastHit2D hit = Physics2D.Raycast(start_position, direction, ray_length, _groundMask);
        Debug.DrawRay(start_position, direction * ray_length, Color.red);
        return hit ? hit : null;
    }

    protected override void InitialState() { _stateManager.Init<MonsterOnLandState>(); }

    public bool IsFrontWall() { return _frontWall; }
    public bool GetMoveToRight() { return _moveToRight; }
    public void TurnAround()
    {
        FlipObject();
        _moveToRight = !_moveToRight;
    }
    public Vector2 ComputeHitForce(Player player)
    {
        Vector2 to_player = player.transform.position - transform.position;
        Vector2 direction = new Vector2(0.5f, 0.5f).normalized;
        direction.x *= Mathf.Sign(to_player.x);
        return direction * _hitForce;
    }
    
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
}
