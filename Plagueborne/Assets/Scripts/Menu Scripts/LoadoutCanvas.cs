using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutCanvas : MonoBehaviour
{
    public MenuCanvas mainCanvas;
    public GameObject backButton;
    void Start()
    {
        backButton.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(mainCanvas.StartMovement(gameObject, new Vector2(40, 0), 2, false)));
    }
}
