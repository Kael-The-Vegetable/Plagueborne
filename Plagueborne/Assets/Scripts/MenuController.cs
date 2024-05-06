using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private GameState _state;
    [field:SerializeField] public GameObject[] Buttons { get; private set; }
    // 0 is Play
    // 1 is LoadOut
    // 2 is Exit
    public EventSystem eventSystem;
    void Awake()
    {
        _state = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    void Start()
    {
        Buttons[0].GetComponent<Button>().onClick.AddListener(() => _state.ChangeScene(1));
        Buttons[1].GetComponent<Button>().onClick.AddListener(LoadOut);
        Buttons[2].GetComponent<Button>().onClick.AddListener(_state.ExitGame);
    }
    private void LoadOut()
    {

        eventSystem.SetSelectedGameObject(Buttons[0]);
    }
}
