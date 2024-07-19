using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadoutCurrent : MonoBehaviour
{
    private TextMeshProUGUI _message;
    void Start()
    {
        _message = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        _message.text = $"Current Loadout:" +
            $"\nMain Attack:{Singleton.Global.State.playerLoadout[0].name}" +
            $"\nSecondary Attack:{Singleton.Global.State.playerLoadout[1].name}";
    }
}
