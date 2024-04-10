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

    protected Strategy.MonsterAI CreateStrategy()
    {
        if (strategyChoice == StrategyChoice.KeepMove) {
            return new Strategy.KeepMove();
        }
        return null;
    }
}
