using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutButton : MonoBehaviour
{
    public GameObject attack;
    public Toggle isMainAttack;

    public void Select()
    {
        Singleton.Global.State.playerLoadout[isMainAttack.onToggle ? 0 : 1] = attack;
    }
}
