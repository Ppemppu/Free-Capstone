using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner; //적의 삭제를 enemyspawner에 알려서 삭제

    [SerializeField]
    private int gold = 10;  //적 사망시 휙득 가능한 골드

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
        if (currentIndex < wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
        }
        else if (currentIndex == wayPointCount - 1)
        {
            transform.position = wayPoints[currentIndex].position;
            currentIndex = 0;
        }

        Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
        movement2D.MoveTo(direction, 1);

        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true; // 왼쪽을 향할 때 이미지 반전
        }
        else if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // 오른쪽을 향할 때 이미지 원래대로
        }
    }

    public void OnDie()
    {

        enemySpawner.DestroyEnemy(this,gold);
    }
}
