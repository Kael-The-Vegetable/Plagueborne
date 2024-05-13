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
        buttons[1].onClick.AddListener(() => StartCoroutine(StartMovement(loadout, new Vector2(0, 0), 2, true)));

        buttons.Add(optionButton.GetComponent<Button>());
        buttons[2].onClick.AddListener(() => StartCoroutine(StartMovement(options, new Vector2(0, 0), 2, true)));

        buttons.Add(exitButton.GetComponent<Button>());
        buttons[3].onClick.AddListener(GameState.ExitGame);
        #endregion
    }
    public IEnumerator StartMovement(GameObject objToMove, Vector2 newPos, float duration, bool disableMenu)
    {
        objToMove.transform.DOMove(newPos, duration);
        if (disableMenu)
        {
            DisableButtons(true);
            objToMove.SetActive(true);
            yield return new WaitForSeconds(duration);
            GameObject selectedObj = objToMove.transform.GetChild(1).gameObject;
            eventSystem.SetSelectedGameObject(selectedObj);
        }
        else
        {
            yield return new WaitForSeconds(duration);
            DisableButtons(false);
            objToMove.SetActive(false);
            eventSystem.SetSelectedGameObject(playButton);
        }
    }
    private void DisableButtons(bool disable)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].enabled = !disable;
        }
    }
}
