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
    private Strategy.MonsterAI _strategy;
    private CapsuleCollider2D _capsuleCollider;
    private Vector2 _capsuleSize;
    protected bool _frontWall;
    public bool _moveToRight = true;

    // Start is called before the first frame update
    public override void Start()
    {
        // Need refactor
        base.Start();
        _state = new MonsterOnLandState();
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
        // Need refactor
        CollectState();
        _frontWall = DetectFrontWall() != null; // Wait refactor, and will be moved into Collectstate()
        _commandList.Add(_strategy.Decide(this));
        BaseState oldState = _state;
        _state = _state.FixedUpdate(gameObject);
        if (!ReferenceEquals(oldState, _state)) { _state.OnStateStart(gameObject); }
        _commandList.Clear();
    }

    public void TurnAround() { _moveToRight = !_moveToRight;}
    public bool GetMoveToRight() { return _moveToRight; }

    protected override RaycastHit2D? DetectGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(_rigidbody.position, -Vector2.up, 0.5f);
        return hit ? hit : null;
    }

    protected override RaycastHit2D? DetectSlope()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        // TODO: now is hard coded, try to extract the parameter to unity property
        RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.5f, 0), -Vector2.up, 1.0f, ground_mask);
        return hit ? hit : null; // check hit.collider is empty or not
    }

    protected RaycastHit2D? DetectFrontWall()
    {
        LayerMask ground_mask = LayerMask.GetMask("Ground");
        Vector2 start_position = _rigidbody.position;
        Vector2 direction = Vector2.right * (GetMoveToRight() ? 1 : -1);
        start_position.y += _capsuleSize.y / 2;
        float ray_length = _capsuleSize.x / 2 + 0.1f;
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

    public bool IsFrontWall() { return _frontWall; }
}
