using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSystem : MonoBehaviour
{
    public static WaveSystem Instance;
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
    private float setWaveTime;
    public PlayerHP playerHP;
    public DefeatBoss uiDefeatBoss;
    private bool isChoosing = false; // �÷��̾ ���� ������ ���θ� ��Ÿ��

    public int CurrentWave => currentWaveIndex + 1;
    public int LeftTime => Mathf.FloorToInt(waveTimeleft);

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        setWaveTime = 120;
        StartWave();
    }
    public void StartWave()
    {
        if (currentWaveIndex < waves.Length - 1)
        { 
            playerHP.resetHP();
            waveTimeleft = setWaveTime; //�� ���̺�� �ð�
            currentWaveIndex++;
            float minRandomHPIncrease = initialMinRandomHPIncrease + (randomHPIncreasePerWave * currentWaveIndex);
            float maxRandomHPIncrease = initialMaxRandomHPIncrease + (randomHPIncreasePerWave * currentWaveIndex);
            enemySpawner.StartWave(waves[currentWaveIndex], minRandomHPIncrease, maxRandomHPIncrease);

        }
    }

    public void Update()
    {
        // ���̺� �ð� ����
        waveTimeleft -= Time.deltaTime;
        // ���̺� �ð� �ʰ�
        if (waveTimeleft < 1)
        {
            if (playerHP.CurrentHP != waves[currentWaveIndex].maxEnemyCount)
            {
                playerHP.GameOver();
            }
            else
            {
                DefeatWave();
            }
        }
        if (playerHP.CurrentHP == waves[currentWaveIndex].maxEnemyCount)
        {
            DefeatWave();
        }
    }


    private void DefeatWave()
    {
        if (waves[currentWaveIndex].isBossWaves && !isChoosing)
        {
            isChoosing = true; // ���� ���·� ��ȯ
            defeatBoss(); // defeatBoss() ȣ�� - ����â ����
        }
        if (!isChoosing)
        {
            StartWave();
        }
    }
    public void CompleteChoice()
    {
        isChoosing = false; // ���� ���� ����
        StartWave(); // ���� ���̺� ����
    }
    public void defeatBoss()
    {
        uiDefeatBoss.Show();
    }



    public int returnMaxEnemy()
    {
        return waves[currentWaveIndex].maxEnemyCount;
    }
    public void increaseWaveTime(float time)
    {
        setWaveTime += time;
    }
}


[System.Serializable]
public struct Wave
{
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
    public bool isBossWaves;
}
