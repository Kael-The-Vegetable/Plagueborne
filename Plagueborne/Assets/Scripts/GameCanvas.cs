using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    public TextMeshProUGUI message;
    void Start()
    {
        
    }
    void Update()
    {
        message.text = $"Kills: {Singleton.Global.State.Kills}";
    }
}
