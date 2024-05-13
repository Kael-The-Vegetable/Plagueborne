using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [field:SerializeField] public GameObject[] MainButtonObjects { get; private set; }
    public Button[] MainButtons { get; private set; }
    [Space]
    
    public GameObject loadOutPanel;
    public GameObject[] LoadOutButtons { get; private set; }
    public Coroutine loadCoroutine;

    public GameObject optionPanel;
    public GameObject[] OptionButtons { get; private set; }
    public Coroutine optionCoroutine;

    public EventSystem eventSystem;
    void Start()
    {

        #region Regular Buttons
        MainButtons = new Button[MainButtonObjects.Length];
        MainButtons[0] = MainButtonObjects[0].GetComponent<Button>();
        MainButtons[0].onClick.AddListener(() => GameState.ChangeScene(1));

        MainButtons[1] = MainButtonObjects[1].GetComponent<Button>();
        MainButtons[1].onClick.AddListener(() => LoadOut(new Vector2(400, 0), LoadOutButtons[0]));

        MainButtons[2] = MainButtonObjects[2].GetComponent<Button>();
        MainButtons[2].onClick.AddListener(() => Options(new Vector2(400, 0), OptionButtons[0]));

        MainButtons[3] = MainButtonObjects[3].GetComponent<Button>();
        MainButtons[3].onClick.AddListener(GameState.ExitGame);
        #endregion

        #region LoadOut Buttons
        int length = loadOutPanel.transform.childCount;
        LoadOutButtons = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            LoadOutButtons[i] = loadOutPanel.transform.GetChild(i).gameObject;
        }
        // final child should be the exit button.
        LoadOutButtons[length - 1].GetComponent<Button>().onClick.AddListener(() => LoadOut(new Vector2(1200, 0), MainButtonObjects[1]));
        #endregion
        
        #region Options Buttons
        length = optionPanel.transform.childCount;
        OptionButtons = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            OptionButtons[i] = optionPanel.transform.GetChild(i).gameObject;
        }
        // final child should be the exit button.
        OptionButtons[length - 1].GetComponent<Button>().onClick.AddListener(() => Options(new Vector2(-400, 0), MainButtonObjects[2]));
        #endregion

        #region Sounds
        for (int i = 0; i < MainButtonObjects.Length; i++) // add blip sound
        { MainButtons[i].onClick.AddListener(Singleton.Global.Sounds.PlayBlip); }
        for (int i = 0; i < LoadOutButtons.Length; i++) // add blip sound
        { LoadOutButtons[i].GetComponent<Button>().onClick.AddListener(Singleton.Global.Sounds.PlayBlip); }
        for (int i = 0; i < OptionButtons.Length; i++) // add blip sound
        { OptionButtons[i].GetComponent<Button>().onClick.AddListener(Singleton.Global.Sounds.PlayBlip); }
        #endregion
    }

    // if a is pressed stop b from pressing
    // if b is pressed stop a from pressing
    private void LoadOut(Vector2 pos, GameObject endSelection)
    {
        if (loadCoroutine != null)
        { StopCoroutine(loadCoroutine); }
        loadCoroutine = StartCoroutine(MovePanel(loadOutPanel.GetComponent<RectTransform>(), pos, 0.05f, 2, endSelection));
    }
    private void Options(Vector2 pos, GameObject endSelection)
    {
        if (optionCoroutine != null)
        { StopCoroutine(optionCoroutine); }
        optionCoroutine = StartCoroutine(MovePanel(optionPanel.GetComponent<RectTransform>(), pos, 0.05f, 2, endSelection));
    }
    private IEnumerator MovePanel(RectTransform transform, Vector2 newPos, float lerpAmount, int minPixelDistance, GameObject selectedButton)
    {

        while (Vector2.Distance(transform.anchoredPosition, newPos) > minPixelDistance)
        {
            Vector2 difference = Vector2.Lerp(Vector2.zero, newPos - transform.anchoredPosition, lerpAmount);
            transform.anchoredPosition += 
                (Vector2.Distance(difference, transform.anchoredPosition) < minPixelDistance) ?
                difference.normalized * minPixelDistance :
                difference;
            yield return null;
        }
        transform.anchoredPosition = newPos;
        eventSystem.SetSelectedGameObject(selectedButton);
    }
}
