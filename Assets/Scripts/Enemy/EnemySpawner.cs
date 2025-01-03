using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField]
   // private GameObject enemyPrefab;
    //[SerializeField]
    //private float spawnTime;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private PlayerGold playerGold;

    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;
    public PlayerHP playerHP;

    private Wave currentWave;
    private float minRandomHPIncrease;
    private float maxRandomHPIncrease;

    private void Awake()
    {
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wave wave, float minRandom, float maxRandom)
    {
        currentWave = wave;
        minRandomHPIncrease = minRandom;
        maxRandomHPIncrease = maxRandom;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        int spawnEnemyCount = 0;
        while (spawnEnemyCount < currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0, currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);//적 생성
            Enemy enemy = clone.GetComponent<Enemy>();
            EnemyHP enemyHP = clone.GetComponent<EnemyHP>();//컴포넌트 받아오기

            // 랜덤 체력 증가 적용
            float baseHP = enemyHP.GetMaxHP();
            float randomHPIncrease = Random.Range(minRandomHPIncrease, maxRandomHPIncrease);
            int totalHP = Mathf.RoundToInt(baseHP + randomHPIncrease);
            enemyHP.SetMaxHP(totalHP);

            enemy.Setup(this, wayPoints);//이동 경로 설정
            enemyList.Add(enemy);
            spawnEnemyCount++;
            yield return new WaitForSeconds(0.4f); //0.4초 기다린 후 SpawnEnemy()
        }
    }

    public void DestroyEnemy(Enemy enemy,int gold)
    {
        playerHP.ChangeHP(1); // 적 제거 시 라이프 값 감소
        playerGold.CurrentGold += gold;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }


    public int Enemyleft()
    {
        return enemyList.Count;
    }
    
}
