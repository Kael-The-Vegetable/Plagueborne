using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public GameObject playButton;
    public GameObject loadoutButton;
    public GameObject optionButton;
    public GameObject exitButton;
    [Space]
    public GameObject loadout;
    public GameObject options;

    private List<Button> buttons = new List<Button>();
    // 0 = playButton
    // 1 = loadoutButton
    // 2 = optionButton
    // 3 = exitButton

    public EventSystem eventSystem;
    void Start()
    {
        #region Regular Buttons
        buttons.Add(playButton.GetComponent<Button>());
        buttons[0].onClick.AddListener(() => GameState.ChangeScene(1));

        buttons.Add(loadoutButton.GetComponent<Button>());
        buttons[1].onClick.AddListener(() => StartCoroutine(StartMovement(loadout, new Vector2(0, 0), 3)));

        buttons.Add(optionButton.GetComponent<Button>());
        buttons[2].onClick.AddListener(() => StartCoroutine(StartMovement(options, new Vector2(0, 0), 3)));

        buttons.Add(exitButton.GetComponent<Button>());
        buttons[3].onClick.AddListener(GameState.ExitGame);
        #endregion
    }
    private IEnumerator StartMovement(GameObject objToMove, Vector2 newPos, float duration)
    {
        DisableButtons(true);
        objToMove.SetActive(true);
        objToMove.transform.DOMove(newPos, duration);
        yield return new WaitForSeconds(duration);
        GameObject selectedObj = objToMove.transform.GetChild(1).gameObject;
        Debug.Log(eventSystem);
        Debug.Log(selectedObj);
        eventSystem.SetSelectedGameObject(selectedObj);
    }
    private void DisableButtons(bool disable)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].enabled = !disable;
        }
    }

    public void GoToMenu(GameObject objtoMove, Vector2 pos, float duration)
    {
        
    }
}
