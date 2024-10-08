using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Waves : MonoBehaviour
{
    // Wave Information
    [Tooltip("WaveLength | WaveSpawnRate | NumberToRepeatNextTwo | MonsterType | MonsterPercentage")]
    [field:SerializeField] public string[] WaveData { get; private set; }

    #region Read Data
    private int[] _waveLength;
    private float[] _waveSpawnRate;
    // first number is starting index in _waveMonsters & _monsterPercentages, second is index to stop at.
    private int[,] _waveTypes;
    private List<string> _waveMonsters;
    private List<float> _monsterPercentages;
    #endregion

    private EnemySpawner _spawner;
    private float _currentWaveTime;
    private int _currentWave = 0;

    private Dictionary<string, ObjectPool> _monsterTypes = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        ReadData();

        SceneManager.sceneLoaded += SceneChanged; // adding the SceneChanged method to the event.

        if (WaveData.Length > 0)
        {
            _spawner = FindFirstObjectByType<EnemySpawner>();
        }
    }

    private void Start()
    {
        if (WaveData.Length > 0)
        {
            #region Dictionary Stuff
            _monsterTypes["Peasant"] = Singleton.Global.Objects.GetPeasantPool();
            _monsterTypes["Slime"] = Singleton.Global.Objects.GetSlimePool();
            #endregion

            StartWave(_currentWave);
        }
    }
    private void Update()
    {
        if (_currentWave < _waveLength.Length)
        {
            _currentWaveTime += Time.deltaTime;
            if (_waveLength[_currentWave] < _currentWaveTime)
            {
                _currentWaveTime = 0;
                StartWave(++_currentWave);
            }
        }
    }
    private void ReadData()
    {
        _waveLength = new int[WaveData.Length];
        _waveSpawnRate = new float[WaveData.Length];
        _waveTypes = new int[WaveData.Length, 2];
        _waveMonsters = new List<string>();
        _monsterPercentages = new List<float>();

        for (int i = 0; i < WaveData.Length; i++)
        {
            string[] brokenString = WaveData[i].Split(" | ");
            // part 1 should be length of wave
            _waveLength[i] = int.Parse(brokenString[0]);
            _waveSpawnRate[i] = float.Parse(brokenString[1]);
            int types = int.Parse(brokenString[2]);
            for (int c = 0; c < types; c++)
            {
                _waveMonsters.Add($"{brokenString[3 + c * 2]}");
                _monsterPercentages.Add(float.Parse(brokenString[4 + c * 2]));
            }
            _waveTypes[i, 0] = _waveMonsters.Count - types;
            _waveTypes[i, 1] = _waveMonsters.Count;
        }
    }
    private void StartWave(int id)
    {
        _spawner.pools.Clear();
        _spawner.poolPercentages.Clear();
        for (int i = _waveTypes[id, 0]; i < _waveTypes[id, 1]; i++)
        {
            if (_monsterTypes.TryGetValue(_waveMonsters[i], out ObjectPool poolDelegate))
            {
                _spawner.pools.Add(poolDelegate);
                _spawner.poolPercentages.Add(_monsterPercentages[i]);
            }
            _spawner.timeBetweenEntities = _waveSpawnRate[id];
        }
    }

    /// <summary>
    /// ONLY USE WHEN ABOUT TO CHANGE SCENES!
    /// </summary>
    /// <param name="data"></param>
    public void SetWaveDataForNextScene(string[] data) => WaveData = data;
    private void SceneChanged(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0)
        {
            ReadData();
            _spawner = FindFirstObjectByType<EnemySpawner>();
            Start();
        }
    }
}
