using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Enemy : MonoBehaviour
{
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex = 0;
    private Movement2D movement2D;
    private EnemySpawner enemySpawner; //적의 삭제를 enemyspawner에 알려서 삭제
    private float moveSpeed=1;
    private float originSpeed;  // 원래 이동 속도 저장
    private bool isSlowed;      // 현재 슬로우 상태인지
    private float slowTimer;    // 현재 슬로우 타이머
    private float slowDuration; // 슬로우 지속시간

    [SerializeField]
    private int gold = 10;  //적 사망시 휙득 가능한 골드

    public float spawnTime;

    private Color originalColor; // 원래 색상 저장
    private SpriteRenderer spriteRenderer; // SpriteRenderer 저장

    public GameObject deathPrefab;

    private void Start()
    {
        spawnTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer 참조 저장
        originalColor = spriteRenderer.color; 
    }

    private void Update()
    {
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;

            // 슬로우 시간이 끝나면 원래 속도로 복구
            if (slowTimer <= 0)
            {
                RemoveSlow();
            }
        }
    }
    public void Setup(EnemySpawner enemySpawner,Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;
        originSpeed = moveSpeed;
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
        movement2D.MoveTo(direction, moveSpeed);

        if (direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true; // 왼쪽을 향할 때 이미지 반전
        }
        else if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // 오른쪽을 향할 때 이미지 원래대로
        }
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        // 이미 슬로우 상태라면 타이머만 리셋
        if (isSlowed)
        {
            slowTimer = duration;
            return;
        }

        isSlowed = true;
        slowTimer = duration;
        slowDuration = duration;

        // 현재 속도에서 slowAmount(0~1)만큼 감소
        moveSpeed = originSpeed * (1 - slowAmount);

        // 슬로우 색상 적용 (파란색으로 변경)
        spriteRenderer.color = new Color(0f, 0f, 1f, 1f);

        // 현재 이동 방향 유지하면서 속도만 변경
        Vector3 currentDirection = (wayPoints[currentIndex].position - transform.position).normalized;
        movement2D.MoveTo(currentDirection, moveSpeed);
    }
    private void RemoveSlow()
    {
        isSlowed = false;
        moveSpeed = originSpeed;

        // 슬로우 풀리면 원래 색상으로 변경
        spriteRenderer.color = originalColor;

        // 현재 이동 방향 유지하면서 원래 속도로 복구
        Vector3 currentDirection = (wayPoints[currentIndex].position - transform.position).normalized;
        movement2D.MoveTo(currentDirection, moveSpeed);
    }

    public void OnDie()
    {
        movement2D.moveSpeed = 0.0f;
        Instantiate(deathPrefab, transform.position, Quaternion.identity); // 사망 이펙트 프리펩 생성
        enemySpawner.DestroyEnemy(this, gold);
    }

    public void deathPrefebDestroy()
    {
        Destroy(gameObject);
    }
}
