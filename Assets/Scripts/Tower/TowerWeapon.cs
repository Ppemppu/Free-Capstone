using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; // �߻�ü ������
    [SerializeField]
    private Transform spawnPoint;        // �߻�ü ���� ��ġ
    [SerializeField]
    private float attackRate = 0.5f;     // ���� �ӵ�
    [SerializeField]
    private float attackRange = 5.0f;    // ���� ����
    [SerializeField]
    private float attackDamage = 1f;        // ���ݷ�
    [SerializeField]
    private bool splashattack = false;      //���÷��� ���� ����
    [SerializeField]
    private float splashDamage = 1f;        //���÷��� ������


    private WeaponState weaponState = WeaponState.SearchTarget; // ������ ����
    private Transform attackTarget = null;   // ���ݴ��
    private EnemySpawner enemySpawner;
    private Tower tower;
    private Animator animator;
    public AudioClip attackSound;
    private float EnhanceDamage;
    private float baseAttackDamage;
    private float baseSplashDamage;
    private float artifactFixedDamageBonus = 0f;  // ���� ������ ����
    private float artifactPercentDamageBonus = 0f; // �ۼ�Ʈ ������ ����

    private void Awake()
    {
        tower = GetComponent<Tower>();
        // Ÿ���� ������ �� �⺻ ������ ���� ����
        baseAttackDamage = attackDamage;
        baseSplashDamage = splashDamage;
    }
    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        // ���� ���¸� WeaponState.SearchTarget���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        // ������ ��� ���̴� ���� ����
        StopCoroutine(weaponState.ToString());

        // ���� ����
        weaponState = newState;

        // ���ο� ���� ���
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
            localScale.x = dx < 0 ? -Mathf.Abs(localScale.x) : Mathf.Abs(localScale.x); // ���⿡ ���� ������ ����
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

            // EnemySpawner�� enemyList �ȿ� �ִ� ���� �ʿ� �����ϴ� ��� �� �˻�
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

            // ���� ��� ����
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
            // Ÿ���� �ִ��� �˻�
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }
            // ������ ���� �����Ÿ� ������ ������ �ٽ� ã��
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }
            animator.SetTrigger("attack");
            // ���� �ӵ� �ð� ��ŭ ���
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
            Debug.LogWarning("�߻�ü ������, ���� ��ġ �Ǵ� ���� ����� null�Դϴ�.");
        }
    }
 
    public void ApplyUpgrades()
    {
        int level = TowerUpgradeManager.Instance.GetUpgradeLevel(tower.Data.Type);
        float increasePerLevel = 0.1f; // ������ 10% ����
        attackDamage = 10 * level + baseAttackDamage * (1f + increasePerLevel * (level - 1));//
        splashDamage =5*level+baseSplashDamage * (1f + increasePerLevel * (level - 1));// ������ 10*x + �⺻������ *(1.05x)

        var (fixedBonus, percentBonus) = TowerUpgradeManager.Instance.GetArtifactEffects(tower.Data.Type);
        attackDamage += fixedBonus;                  // ���� ���� ����
        attackDamage *= 1f + (percentBonus / 100f);  // �ۼ�Ʈ ���� ����
        splashDamage *= 1f + (percentBonus / 100f);
    }
    public void PlayAttackSound()
    {
        if (attackSound != null)
        {
            SoundManager.Instance.PlaySound(attackSound, 0.1f);
        }
    }

}