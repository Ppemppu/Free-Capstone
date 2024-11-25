using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public DefeatBoss uiDefeatBoss;

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
            playerHP.resetHP();
            waveTimeleft = 120; //한 웨이브당 시간
            currentWaveIndex++;
            float minRandomHPIncrease = initialMinRandomHPIncrease + (randomHPIncreasePerWave * currentWaveIndex);
            float maxRandomHPIncrease = initialMaxRandomHPIncrease + (randomHPIncreasePerWave * currentWaveIndex);
            enemySpawner.StartWave(waves[currentWaveIndex], minRandomHPIncrease, maxRandomHPIncrease);
        }
    }
    public void Update()
    {
        waveTimeleft -= Time.deltaTime;
        if (waveTimeleft < 1)
        {
            if (playerHP.CurrentHP != waves[currentWaveIndex].maxEnemyCount)
                playerHP.GameOver();
            else
            {
                if (waves[currentWaveIndex].isBossWaves)
                    defeatBoss();
                StartWave();
            }
        }
        if (playerHP.CurrentHP == waves[currentWaveIndex].maxEnemyCount)
        {
            if (waves[currentWaveIndex].isBossWaves)
                defeatBoss();

            StartWave();
        }
    }
    public void defeatBoss()
    {
        uiDefeatBoss.Show();
    }
    public int returnMaxEnemy()
    {
        return waves[currentWaveIndex].maxEnemyCount;
    }
}


[System.Serializable]
public struct Wave
{
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
    public bool isBossWaves;
}
