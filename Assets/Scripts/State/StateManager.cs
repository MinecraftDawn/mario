using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State {

public class StateManager
{
    // Store Flyweight state object; add new object when first visit.
    private Dictionary<string, BaseState> _states;
    private BaseState _currentState;

    public void Init<T>() where T : BaseState, new()
    {
        _states = new Dictionary<string, BaseState>();
        _currentState = new T();
        _states.Add(_currentState.GetType().FullName, _currentState);
    }

    public T StateTransition<T>() where T : BaseState, new()
    {
        if (!_states.ContainsKey(typeof(T).FullName)) {
            _states.Add(typeof(T).FullName, new T());
        }
        _states[typeof(T).FullName].Reset();
        _currentState = _states[typeof(T).FullName];
        return (T)_currentState;
    }

    public BaseState GetCurrentState() { return _currentState; }
}

}