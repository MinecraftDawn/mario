using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event;

public class GameContext : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameOverInterface;
    [SerializeField]
    private GameObject _gameFinishInterface;
    [SerializeField]
    private InputHandler _inputHandler;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private int score;
    public static EventQueue eventQueue;
    public int maxExecuteCount = 5;
    public bool isGameMode = false;

    void Start()
    {
        eventQueue = new EventQueue();
        _gameOverInterface.SetActive(false);
    }

    void Update()
    {
        int count = 0;
        while (!eventQueue.IsEmpty() && count < maxExecuteCount) {
            EventBase game_event = eventQueue.Dequeue();
            game_event.UpdateContext(this);
            count++;
        }
    }

    public void AddScore(int value) { score += value; }
    public void GameOver()
    {
        _gameOverInterface.SetActive(true);
        _inputHandler.DisablePlayerControl();
    }

    public void GameFinish()
    {
        _gameFinishInterface.SetActive(true);
        _inputHandler.DisablePlayerControl();
    }
    public GameObject GetPlayer() { return _player; }
}
