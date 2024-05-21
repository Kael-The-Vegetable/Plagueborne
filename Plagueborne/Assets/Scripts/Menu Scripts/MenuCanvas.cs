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
    [Header("Menus")]
    public GameObject play;
    public GameObject selectedPlayObj;
    [Space]
    public GameObject loadout;
    public GameObject selectedLoadoutObj;
    [Space]
    public GameObject options;
    public GameObject selectedOptionsObj;
    
    private List<Button> buttons = new List<Button>();
    // 0 = playButton
    // 1 = loadoutButton
    // 2 = optionButton
    // 3 = exitButton
    [Space]
    public EventSystem eventSystem;
    void Start()
    {
        #region Regular Buttons
        buttons.Add(playButton.GetComponent<Button>());
        buttons[0].onClick.AddListener(() => StartCoroutine(StartMovement(play, new Vector2(0, 0), 2, true, selectedPlayObj)));

        buttons.Add(loadoutButton.GetComponent<Button>());
        buttons[1].onClick.AddListener(() => StartCoroutine(StartMovement(loadout, new Vector2(0, 0), 2, true, selectedLoadoutObj)));

        buttons.Add(optionButton.GetComponent<Button>());
        buttons[2].onClick.AddListener(() => StartCoroutine(StartMovement(options, new Vector2(0, 0), 2, true, selectedOptionsObj)));

        buttons.Add(exitButton.GetComponent<Button>());
        buttons[3].onClick.AddListener(GameState.ExitGame);
        #endregion
    }
    public IEnumerator StartMovement(GameObject objToMove, Vector2 newPos, float duration, bool disableMenu, GameObject newSelectedItem = null)
    {
        
        objToMove.transform.DOMove(newPos, duration);
        if (disableMenu)
        {
            DisableButtons(true);
            objToMove.SetActive(true);
            yield return new WaitForSeconds(duration);
            GameObject selectedObj;
            if (newSelectedItem == null)
            { selectedObj = objToMove.transform.GetChild(1).gameObject; }
            else 
            { selectedObj = newSelectedItem; }
            eventSystem.SetSelectedGameObject(selectedObj);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
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
