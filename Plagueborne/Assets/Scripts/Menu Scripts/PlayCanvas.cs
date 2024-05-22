using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayCanvas : MonoBehaviour
{
    public MenuCanvas mainCanvas;
    public GameObject backButton;
    public GameObject scrollableArea;
    public EventSystem eventSystem;
    public GameObject[] buttons;

    private GameObject _prevSelectedObj;
    private RectTransform _prevRectTransform;
    private RectTransform _scrollableAreaRectTransform;
    private float _topPadding;
    private RectTransform _rectTransform;
    private void OnEnable()
    {
        _prevSelectedObj = eventSystem.currentSelectedGameObject;
    }
    void Start()
    {
        backButton.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(mainCanvas.StartMovement(gameObject, new Vector2(0, -25), 2, false)));
        _scrollableAreaRectTransform = scrollableArea.GetComponent<RectTransform>();
        _rectTransform = GetComponent<RectTransform>();
        _topPadding = scrollableArea.GetComponent<VerticalLayoutGroup>().padding.top;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Button>().onClick.AddListener(() => PlayLevel1());
        }
    }
    private void Update()
    {

        // this if statement asks if the current selected item is below the screen or above it
        // if it is it will correct the scrollable area so that it is fully in the screen.
        #region Scrolling Fix
        GameObject currentItem = eventSystem.currentSelectedGameObject;
        if (currentItem != null
        && currentItem != backButton
        && _prevSelectedObj != currentItem)
        {
            _prevSelectedObj = currentItem;
            _prevRectTransform = _prevSelectedObj.GetComponent<RectTransform>();
            _scrollableAreaRectTransform = scrollableArea.GetComponent<RectTransform>();

            float posOfScrollableContent = _scrollableAreaRectTransform.anchoredPosition.y + _scrollableAreaRectTransform.rect.yMax * 0.5f;

            float bottomOfObj = 
                _prevRectTransform.rect.yMin * 2 
                + _prevRectTransform.anchoredPosition.y 
                + posOfScrollableContent;

            float topOfObj = _prevRectTransform.anchoredPosition.y + posOfScrollableContent;

            if (topOfObj > -_topPadding)
            {
                float difference = topOfObj + _topPadding;
                float newPosY = _scrollableAreaRectTransform.anchoredPosition.y - difference;
                _scrollableAreaRectTransform.DOAnchorPosY(newPosY, 1);
            }
            else if (bottomOfObj < _rectTransform.rect.yMin * 2)
            {
                float difference = _rectTransform.rect.yMin * 2 - bottomOfObj; // positive
                float newPosY = _scrollableAreaRectTransform.anchoredPosition.y + difference;
                _scrollableAreaRectTransform.DOAnchorPosY(newPosY, 1);
            }
        }
        #endregion
    }
    private void PlayLevel1()
    {
        string[] waveData =
        {
            "60 | 0.05 | 1 | Peasant | 1",
            "60 | 1 | 1 | Slime | 1",
            "60 | 0.05 | 2 | Slime | 0.05 | Peasant | 0.95",
            "120 | 0.01 | 2 | Slime | 0.95 | Peasant | 0.05"
        };
        Singleton.Global.Waves.SetWaveDataForNextScene(waveData);
        GameState.ChangeScene(1);
    }
}
