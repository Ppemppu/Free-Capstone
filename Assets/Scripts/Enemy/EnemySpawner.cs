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

    private void Awake()
    {
        enemyList = new List<Enemy>();
    }

    public void StartWave(Wave wave)
    {
        currentWave = wave;
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        //���� ���̺꿡�� ���� ������ ����
        int spawnEnemyCount = 0;
        while (spawnEnemyCount<currentWave.maxEnemyCount)
        {
            int enemyIndex = Random.Range(0,currentWave.enemyPrefabs.Length);
            GameObject clone = Instantiate(currentWave.enemyPrefabs[enemyIndex]);
            Enemy enemy = clone.GetComponent<Enemy>();
            enemy.Setup(this,wayPoints);
            enemyList.Add(enemy);
            playerHP.ChangeHP(1); // �� ���� �� HP ����
            spawnEnemyCount++;
            yield return new WaitForSeconds(currentWave.spawnTime);
        }
    }
    public void DestroyEnemy(Enemy enemy,int gold)
    {
        playerHP.ChangeHP(-1); // �� ���� �� HP ����
        playerGold.CurrentGold += gold;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }


    public int Enemyleft()
    {
        return enemyList.Count;
    }
    
}
