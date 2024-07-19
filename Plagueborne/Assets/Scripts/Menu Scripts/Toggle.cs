using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public string onText;
    public string offText;
    public bool onToggle = false;
    private TextMeshProUGUI _text;
    void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void Flip()
    {
        onToggle = !onToggle;
        _text.text = onToggle ? onText : offText;
    }
}
