using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private GameObject _canvas = null;
    private GameObject _player = null;

    private bool _firstScene;
    private bool _isPaused = false;

    public int Kills { get; set; }

    public GameObject[] playerLoadout = new GameObject[2];

    private void Awake()
    {
        _firstScene = SceneManager.GetActiveScene().buildIndex == 0;
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (!_firstScene)
        { _canvas.SetActive(false); }
        SceneManager.activeSceneChanged += SceneChanged; // adding the SceneChanged method to the event.
    }

    /// <summary>
    /// Only use this method if it is not on the menu scene and currently is in play.
    /// <br />
    /// This method will activate / deactivate the canvas.
    /// </summary>
    /// <param name="pause"></param>
    public void PauseGame()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0.0f : 1.0f;
        _canvas.SetActive(_isPaused);
    }

    /// <summary>
    /// Use this method to change the scene as per the build order.
    /// </summary>
    /// <param name="index"></param>
    public static void ChangeScene(int index) => SceneManager.LoadScene(index);
    
    /// <summary>
    /// Use this method to immediatly exit the program.
    /// </summary>
    public static void ExitGame() => Application.Quit();

    /// <summary>
    /// Use this method to set a variable followed by waiting, then setting the variable later to a different value. <br />
    /// Use ( result => [ your variable name here ] = result ) in place for the variable area.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="variable"></param>
    /// <param name="time"></param>
    /// <param name="initialValue"></param>
    /// <param name="finalValue"></param>
    /// <returns></returns>
    public static IEnumerator DelayedVarChange<T>(Action<T> variable, float time, T initialValue, T finalValue)
    {
        variable(initialValue);
        yield return new WaitForSeconds(time);
        variable(finalValue);
    }


    private void SceneChanged(Scene old, Scene next)
    {
        if (next.buildIndex == 0)
        { _firstScene = true; }
        else
        { _firstScene = false; }
        GameObject[] newObjects = next.GetRootGameObjects();
        _canvas = null;
        _player = null;
        for (int i = 0; i < newObjects.Length && (_canvas == null || _player == null); i++)
        {
            if (newObjects[i].CompareTag("Canvas"))
            { _canvas = newObjects[i]; }
            else if (newObjects[i].CompareTag("Player"))
            { 
                _player = newObjects[i];
                LoadoutSetup(_player.transform); 
            }
        }
        if (!_firstScene && _canvas != null)
        { _canvas.SetActive(false); }
    }
    private void LoadoutSetup(Transform player)
    {
        if (player == null || playerLoadout == null || playerLoadout.Length < 2)
        { return; }
        PlayerController playerScript = player.GetComponent<PlayerController>();
        GameObject mainAttack = Instantiate(playerLoadout[0], player);
        Debug.Log(mainAttack.name);
        playerScript.mainAttack = mainAttack;
        GameObject secondaryAttack = Instantiate(playerLoadout[1], player);
        Debug.Log(secondaryAttack.name);
        playerScript.secondaryAttack = secondaryAttack;
    }
}
