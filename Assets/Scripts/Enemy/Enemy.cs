using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner;

    public float spawnTime;


    private void Start()
    {
       spawnTime= Time.time; 
    }
    public void Setup(EnemySpawner enemySpawner,Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        //적 이동경로 waypoint 설정
        wayPointCount=wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints= wayPoints;

        //적의 위치를 첫번째 wayPoint위치로 설정
        transform.position = wayPoints[currentIndex].position;

        //적 이동/목표지점 설정 코루틴 시작
        StartCoroutine("OnMove");
    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }
            yield return null;
        }
    }

    private void NextMoveTo()
    {
        if(currentIndex < wayPointCount-1) 
        {
          
           transform.position = wayPoints[currentIndex].position;
            currentIndex++; 
            Vector3 direction = (wayPoints[currentIndex].position-transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else if(currentIndex == wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex = 0;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
      
    }

    public void OnDie()
    {
        enemySpawner.DestroyEnemy(this);
    }
}
