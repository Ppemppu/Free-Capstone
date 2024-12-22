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
    private EnemySpawner enemySpawner; //���� ������ enemyspawner�� �˷��� ����
    private float moveSpeed=1;
    private float originSpeed;  // ���� �̵� �ӵ� ����
    private bool isSlowed;      // ���� ���ο� ��������
    private float slowTimer;    // ���� ���ο� Ÿ�̸�
    private float slowDuration; // ���ο� ���ӽð�

    [SerializeField]
    private int gold = 10;  //�� ����� �׵� ������ ���

    public float spawnTime;

    private Color originalColor; // ���� ���� ����
    private SpriteRenderer spriteRenderer; // SpriteRenderer ����

    public GameObject deathPrefab;

    private void Start()
    {
        spawnTime = Time.time;
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ���� ����
        originalColor = spriteRenderer.color; 
    }

    private void Update()
    {
        if (isSlowed)
        {
            slowTimer -= Time.deltaTime;

            // ���ο� �ð��� ������ ���� �ӵ��� ����
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
        //�� �̵���� waypoint ����
        wayPointCount=wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints= wayPoints;

        //���� ��ġ�� ù��° wayPoint��ġ�� ����
        transform.position = wayPoints[currentIndex].position;

        //�� �̵�/��ǥ���� ���� �ڷ�ƾ ����
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
            GetComponent<SpriteRenderer>().flipX = true; // ������ ���� �� �̹��� ����
        }
        else if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false; // �������� ���� �� �̹��� �������
        }
    }

    public void ApplySlow(float slowAmount, float duration)
    {
        // �̹� ���ο� ���¶�� Ÿ�̸Ӹ� ����
        if (isSlowed)
        {
            slowTimer = duration;
            return;
        }

        isSlowed = true;
        slowTimer = duration;
        slowDuration = duration;

        // ���� �ӵ����� slowAmount(0~1)��ŭ ����
        moveSpeed = originSpeed * (1 - slowAmount);

        // ���ο� ���� ���� (�Ķ������� ����)
        spriteRenderer.color = new Color(0f, 0f, 1f, 1f);

        // ���� �̵� ���� �����ϸ鼭 �ӵ��� ����
        Vector3 currentDirection = (wayPoints[currentIndex].position - transform.position).normalized;
        movement2D.MoveTo(currentDirection, moveSpeed);
    }
    private void RemoveSlow()
    {
        isSlowed = false;
        moveSpeed = originSpeed;

        // ���ο� Ǯ���� ���� �������� ����
        spriteRenderer.color = originalColor;

        // ���� �̵� ���� �����ϸ鼭 ���� �ӵ��� ����
        Vector3 currentDirection = (wayPoints[currentIndex].position - transform.position).normalized;
        movement2D.MoveTo(currentDirection, moveSpeed);
    }

    public void OnDie()
    {
        movement2D.moveSpeed = 0.0f;
        Instantiate(deathPrefab, transform.position, Quaternion.identity); // ��� ����Ʈ ������ ����
        enemySpawner.DestroyEnemy(this, gold);
    }

    public void deathPrefebDestroy()
    {
        Destroy(gameObject);
    }
}
