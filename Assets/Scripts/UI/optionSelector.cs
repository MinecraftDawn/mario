using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class optionSelector : MonoBehaviour {

    [FormerlySerializedAs("start")] [SerializeField]
    private Button _startBtn;
    [FormerlySerializedAs("option")] [SerializeField]
    private Button _optionBtn;
    [FormerlySerializedAs("quit")] [SerializeField]
    private Button _quitBtn;
    [FormerlySerializedAs("startScreen")] [SerializeField] 
    private string _startScreenName;
    
    private Button[] buttons;
    private int currentIndex = 0;
    
    void Start()
    {
        buttons = new Button[] { _startBtn, _optionBtn, _quitBtn };
        EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, 
                new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            currentIndex = (currentIndex + 1) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            currentIndex = (currentIndex - 1 + buttons.Length) % buttons.Length;
            EventSystem.current.SetSelectedGameObject(buttons[currentIndex].gameObject);
        }
    }

    public void OnStartButtonPressed() 
    {
        SceneManager.LoadScene(_startScreenName);
    }

    public void OnOptionButtonPressed()
    {
        Debug.Log("Option selected");
    }

    public void OnExitButtonPressed()
    {
        Debug.Log("exit selected");
    }
}
