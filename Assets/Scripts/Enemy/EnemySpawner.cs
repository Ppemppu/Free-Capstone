using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float spawnTime;
    [SerializeField]
    private Transform[] wayPoints;
    [SerializeField]
    private PlayerGold playerGold;

    private List<Enemy> enemyList;
    public List<Enemy> EnemyList => enemyList;
    public PlayerHP playerHP;

    private void Awake()
    {
        enemyList = new List<Enemy>();
        StartCoroutine("SpawnEnemy");
    }
    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            GameObject clone = Instantiate(enemyPrefab);
            Enemy enemy = clone.GetComponent<Enemy>();
            enemy.Setup(this, wayPoints);
            enemyList.Add(enemy);
            playerHP.ChangeHP(1); 
            yield return new WaitForSeconds(spawnTime);
        }
    }
    public void DestroyEnemy(Enemy enemy,int gold)
    {
        playerHP.ChangeHP(-1); // 적 제거 시 HP 증가
        playerGold.CurrentGold += gold;
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }


    public int Enemyleft()
    {
        return enemyList.Count;
    }
    
}
