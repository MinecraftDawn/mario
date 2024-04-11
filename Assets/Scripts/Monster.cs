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

    // Start is called before the first frame update
    public override void Start()
    {
        // Need refactor
        base.Start();
        _state = new MonsterOnLandState();
        _strategy = CreateStrategy();
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
        _commandList.Add(_strategy.Decide(this));
        BaseState oldState = _state;
        _state = _state.FixedUpdate(gameObject);
        if (!ReferenceEquals(oldState, _state)) { _state.OnStateStart(gameObject); }
        _commandList.Clear();
    }

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

    protected Strategy.MonsterAI CreateStrategy()
    {
        if (strategyChoice == StrategyChoice.KeepMove) {
            return new Strategy.KeepMove();
        }
        return null;
    }
}
