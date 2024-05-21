using System;
using System.Collections.Generic;
using UnityEngine;

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
    private List<Transform> _waveMonsters;
    private List<float> _monsterPercentages;
    #endregion

    private int _currentWave = 0;

    private void Awake()
    {
        #region Reading Data

        _waveLength = new int[WaveData.Length];
        _waveSpawnRate = new float[WaveData.Length];
        _waveTypes = new int[WaveData.Length,2];
        _waveMonsters = new List<Transform>();
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
                _waveMonsters.Add(Resources.Load<Transform>($"Prefabs/Enemies/{brokenString[3 + c * 2]}"));
                _monsterPercentages.Add(float.Parse(brokenString[4 + c * 2]));
            }
            _waveTypes[i, 0] = _waveMonsters.Count - types;
            _waveTypes[i, 1] = _waveMonsters.Count;
            Debug.Log($"Length: {_waveLength[i]} SpawnRate: {_waveSpawnRate[i]}");
            for (int j = _waveTypes[i, 0]; j < _waveTypes[i, 1]; j++)
            {
                Debug.Log($"Monster {j} Name: {_waveMonsters[j]} | Percentage: {_monsterPercentages[j]}");
            }
        }
        #endregion
    }
}
