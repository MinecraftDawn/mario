using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;
using State;

public class Monster : ActorBase
{
    public enum StrategyChoice
    {
        KeepMove,
    }

    public StrategyChoice strategyChoice;
    public float frontWallDetectRayLength = 0.1f;
    public Detector detector;
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
        _strategy = CreateStrategy();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _capsuleSize = _capsuleCollider.size;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        _commandList.Clear();
    }

    protected override void PreparationBeforeFixedUpdate()
    {
        CollectState();
        _commandList.Add(_strategy.Decide(this));
    }

    protected override void CollectState()
    {
        base.CollectState();
        _frontWall = DetectFrontWall() != null;
        if (detector.IsDetected()) { Debug.Log(detector.GetDetectedObject()); }
    }

    protected RaycastHit2D? DetectFrontWall()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        Vector2 start_position = _rigidbody.position;
        Vector2 direction = Vector2.right * (GetMoveToRight() ? 1 : -1);
        start_position.y += _capsuleSize.y / 2;
        float ray_length = _capsuleSize.x / 2 + frontWallDetectRayLength;
        RaycastHit2D hit = Physics2D.Raycast(start_position, direction, ray_length, ground_mask);
        Debug.DrawRay(start_position, direction * ray_length, Color.red);
        return hit ? hit : null;
    }

    protected Strategy.MonsterAI CreateStrategy()
    {
        if (strategyChoice == StrategyChoice.KeepMove) {
            return new Strategy.KeepMove();
        }
        return null;
    }

    protected override BaseState InitialState() { return new MonsterOnLandState(); }

    public bool IsFrontWall() { return _frontWall; }
    public bool GetMoveToRight() { return _moveToRight; }
    public void TurnAround()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
        _moveToRight = !_moveToRight;
    }
}
