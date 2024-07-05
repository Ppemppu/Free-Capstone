using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private EnemySpawner enemySpawner;
    private int currentWaveIndex = -1;
    private float waveTimeleft; 

    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;


    public void Start()
    {
        StartWave();
    }
    public void StartWave()
    {
        if ( currentWaveIndex < waves.Length - 1)
        {
            waveTimeleft = 10;

            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
        }
    }
    public void Update()
    {
        waveTimeleft -= Time.deltaTime;
        if (waveTimeleft <= 0)
            StartWave();
    }
}

[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
