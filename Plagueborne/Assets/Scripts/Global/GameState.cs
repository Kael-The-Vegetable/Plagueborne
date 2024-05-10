using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private GameObject _canvas = null;
    private bool _firstScene;
    private bool _isPaused = false;

    public int Kills { get; set; }
    private void Awake()
    {
        _firstScene = SceneManager.GetActiveScene().buildIndex == 0;
        _canvas = GameObject.FindGameObjectWithTag("Canvas");
        if (!_firstScene)
        { _canvas.SetActive(false); }
        SceneManager.activeSceneChanged += SceneChanged;
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

    
    public static IEnumerator DelayedVarChange<T>(Action<T> variable, float time, T initialValue, T finalValue)
    {
        Debug.Log(": Start time: " + Time.time);
        variable(initialValue);
        yield return new WaitForSeconds(time);
        variable(finalValue);
        Debug.Log(": End time: " + Time.time);
    }


    private void SceneChanged(Scene old, Scene next)
    {
        if (next.buildIndex == 0)
        { _firstScene = true; }
        else
        { _firstScene = false; }
        GameObject[] newObjects = next.GetRootGameObjects();
        for (int i = 0; i < newObjects.Length && _canvas == null; i++)
        {
            if (newObjects[i].CompareTag("Canvas"))
            { _canvas = newObjects[i]; }
        }
        if (!_firstScene && _canvas != null)
        { _canvas.SetActive(false); }
    }
    
}
