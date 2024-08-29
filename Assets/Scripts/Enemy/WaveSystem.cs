using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private float initialMinRandomHPIncrease = 5f;
    [SerializeField]
    private float initialMaxRandomHPIncrease = 20f;
    [SerializeField]
    private float randomHPIncreasePerWave = 5f;
    private int currentWaveIndex = -1;
    private float waveTimeleft;
    public PlayerHP playerHP;

    public int CurrentWave => currentWaveIndex + 1;
    public int LeftTime => Mathf.FloorToInt(waveTimeleft);


    public void Start()
    {
        StartWave();
    }
    public void StartWave()
    {
        if (currentWaveIndex < waves.Length - 1)
        {
            waveTimeleft = 80; //한 웨이브당 시간
            currentWaveIndex++;
            float minRandomHPIncrease = initialMinRandomHPIncrease + (randomHPIncreasePerWave * currentWaveIndex);
            float maxRandomHPIncrease = initialMaxRandomHPIncrease + (randomHPIncreasePerWave * currentWaveIndex);
            enemySpawner.StartWave(waves[currentWaveIndex], minRandomHPIncrease, maxRandomHPIncrease);
            Debug.Log($"Wave {CurrentWave} started. Random HP increase range: {minRandomHPIncrease} - {maxRandomHPIncrease}");
        }
    }
    public void Update()
    {
        waveTimeleft -= Time.deltaTime;
        if (waveTimeleft <= 0)
            StartWave();
        if (playerHP.CurrentHP == 0)
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
