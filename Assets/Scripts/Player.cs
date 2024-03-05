using System.Collections;
using System.Collections.Generic;
using Command;
using State;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BaseState _state;
    private List<BaseCommand> _command_list;

    // Start is called before the first frame update
    void Start()
    {
        _state = new OnLandState();
        _command_list = new List<BaseCommand>();
    }

    // Update is called once per frame
    void Update()
    {
        _state = _state.Update(gameObject);
    }

    void FixedUpdate()
    {
        _state = _state.FixedUpdate(gameObject);
    }

    public BaseCommand GetCommand(int idx) { return _command_list[idx]; }
    public int GetCommandListSize() { return _command_list.Count; }
    public void CleanCommandList() { _command_list.Clear(); }
    public IEnumerator GetCommandListEnumerator() { return _command_list.GetEnumerator(); }
    public void ReceiveCommands(BaseCommand command) { _command_list.Add(command); }
    public void ExecuteCommand(int idx) { _command_list[idx].Execute(gameObject); }
}
