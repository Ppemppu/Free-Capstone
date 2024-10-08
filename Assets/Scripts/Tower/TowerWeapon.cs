using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // 발사체 프리팹
    [SerializeField]
    private Transform spawnPoint;        // 발사체 생성 위치
    [SerializeField]
    private float attackRate = 0.5f;     // 공격 속도
    [SerializeField]
    private float attackRange = 5.0f;    // 공격 범위
    [SerializeField]
    private float attackDamage = 1f;        // 공격력
    [SerializeField]
    private bool splashattack = false;      //스플래시 공격 여부
    [SerializeField]
    private float splashDamage = 1f;        //스플래시 데미지


    private WeaponState weaponState = WeaponState.SearchTarget; // 무기의 상태
    private Transform attackTarget = null;   // 공격대상
    private EnemySpawner enemySpawner;
    private Tower tower;
    private Animator animator;
    private float EnhanceDamage;
    private float baseAttackDamage;
    private float baseSplashDamage;

    private void Awake()
    {
        tower = GetComponent<Tower>();
        // 타워가 생성될 때 기본 데미지 값을 저장
        baseAttackDamage = attackDamage;
        baseSplashDamage = splashDamage;
    }
    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        // 최초 상태를 WeaponState.SearchTarget으로 설정
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        // 이전에 재생 중이던 상태 종료
        StopCoroutine(weaponState.ToString());

        // 상태 변경
        weaponState = newState;

        // 새로운 상태 재생
        StartCoroutine(newState.ToString());
    }

    private void Start()
    {
        ApplyUpgrades();
        animator = GetComponent<Animator>();
        ChangeState(WeaponState.SearchTarget);
    }

    private void Update()
    {
        if (attackTarget != null)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        if (attackTarget != null)
        {
            float dx = attackTarget.position.x - transform.position.x;

            Vector2 localScale = transform.localScale;
            localScale.x = dx < 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x); // 방향에 따라 스케일 변경
            transform.localScale = localScale;
        }
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }
            yield return null;

            float oldestSpawnTime = Mathf.Infinity;
            Transform oldestEnemy = null;

            // EnemySpawner의 enemyList 안에 있는 현재 맵에 존재하는 모든 적 검사
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                if (distance <= attackRange)
                {
                    Enemy enemy = enemySpawner.EnemyList[i].GetComponent<Enemy>();
                    if (enemy != null && enemy.spawnTime < oldestSpawnTime)
                    {
                        oldestSpawnTime = enemy.spawnTime;
                        oldestEnemy = enemy.transform;
                    }
                }
            }

            // 공격 대상 설정
            if (oldestEnemy != null)
            {
                attackTarget = oldestEnemy;
            }
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            // 타겟이 있는지 검사
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }
            // 타겟이 공격 범위 안에 있는지 검사
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }

            animator.SetTrigger("attack");
            //SpawnProjectile(); // 공격(발사체 생성) , 이거 애니메이션에 Event로 호출할거라서 주석처리함

            // 공격 속도 시간 만큼 대기
            yield return new WaitForSeconds(attackRate);
        }
    }

    public void SpawnProjectile()
    {
        if (projectilePrefab != null && spawnPoint != null && attackTarget != null)
        {
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            clone.GetComponent<Projectile>().Setup(attackTarget, attackDamage, splashattack, splashDamage, 0.75f);
        }
        else
        {
            Debug.LogWarning("발사체 프리팹, 생성 위치 또는 공격 대상이 null입니다.");
        }
    }
 
    public void ApplyUpgrades()
    {
        int level = TowerUpgradeManager.Instance.GetUpgradeLevel(tower.Data.Type);
        float increasePerLevel = 0.1f; // 레벨당 10% 증가
        attackDamage = 10 * level + baseAttackDamage * (1f + increasePerLevel * (level - 1));//
        splashDamage =5*level+baseSplashDamage * (1f + increasePerLevel * (level - 1));// 레벨당 10*x + 기본데미지 *(1.05x)
    }
}