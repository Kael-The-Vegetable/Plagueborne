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
    [Space]
    public GameObject loadOutPanel;
    [field:SerializeField] public GameObject[] LoadOutButtons { get; private set; }
    public EventSystem eventSystem;
    void Awake()
    {
        _state = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
    }
    void Start()
    {
        #region Regular Buttons
        Buttons[0].GetComponent<Button>().onClick.AddListener(() => _state.ChangeScene(1));
        Buttons[1].GetComponent<Button>().onClick.AddListener(LoadOut);
        Buttons[3].GetComponent<Button>().onClick.AddListener(_state.ExitGame);
        #endregion

        #region LoadOut Buttons
        LoadOutButtons[5].GetComponent<Button>().onClick.AddListener(ExitLoadOut);
        #endregion
    }
    private void LoadOut()
    {
        StopAllCoroutines();
        StartCoroutine(MovePanel(loadOutPanel.GetComponent<RectTransform>(), new Vector2(400, 0), 0.05f, 1, LoadOutButtons[0]));
    }
    private void ExitLoadOut()
    {
        StopAllCoroutines();
        StartCoroutine(MovePanel(loadOutPanel.GetComponent<RectTransform>(), new Vector2(1200, 0), 0.05f, 1, Buttons[1]));
    }
    private IEnumerator MovePanel(RectTransform transform, Vector2 newPos, float lerpAmount, int minPixelDistance, GameObject selectedButton)
    {
        while (Vector2.Distance(transform.anchoredPosition, newPos) > minPixelDistance)
        {
            Vector2 difference = Vector2.Lerp(Vector2.zero, newPos - transform.anchoredPosition, lerpAmount);
            if (Vector2.Distance(difference, transform.anchoredPosition) < minPixelDistance)
            { transform.anchoredPosition += difference.normalized * minPixelDistance; }
            else
            { transform.anchoredPosition += difference; }
            yield return null;
        }
        transform.anchoredPosition = newPos;
        eventSystem.SetSelectedGameObject(selectedButton);
    }
}
