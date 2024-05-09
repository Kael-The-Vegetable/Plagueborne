using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [field:SerializeField] public GameObject[] Buttons { get; private set; }
    [Space]
    public GameObject loadOutPanel;
    public GameObject[] LoadOutButtons { get; private set; }
    public EventSystem eventSystem;
    void Start()
    {
        
        #region Regular Buttons
        Buttons[0].GetComponent<Button>().onClick.AddListener(() => Singleton.Global.State.ChangeScene(1));
        Buttons[1].GetComponent<Button>().onClick.AddListener(LoadOut);
        Buttons[3].GetComponent<Button>().onClick.AddListener(Singleton.Global.State.ExitGame);
        
        #endregion

        #region LoadOut Buttons
        int length = loadOutPanel.transform.childCount;
        LoadOutButtons = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            LoadOutButtons[i] = loadOutPanel.transform.GetChild(i).gameObject;
        }
        // final child should be the exit button.
        LoadOutButtons[length - 1].GetComponent<Button>().onClick.AddListener(ExitLoadOut);
        #endregion

        #region Sounds
        for (int i = 0; i < Buttons.Length; i++) // add blip sound
        { Buttons[i].GetComponent<Button>().onClick.AddListener(Singleton.Global.Sounds.PlayBlip); }
        for (int i = 0; i < length; i++) // add blip sound
        { LoadOutButtons[i].GetComponent<Button>().onClick.AddListener(Singleton.Global.Sounds.PlayBlip); }

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
