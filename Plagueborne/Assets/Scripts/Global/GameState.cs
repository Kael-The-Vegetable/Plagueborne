using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    /// <summary>
    /// Use this method to change the scene as per the build order.
    /// </summary>
    /// <param name="index"></param>
    public void ChangeScene(int index) => SceneManager.LoadScene(index);
    
    /// <summary>
    /// Use this method to immediatly exit the program.
    /// </summary>
    public void ExitGame() => Application.Quit();
    
}
