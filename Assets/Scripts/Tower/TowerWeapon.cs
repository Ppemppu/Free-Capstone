using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WeaponState { SearchTarget = 0, AttackToTarget }
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; //�߻�ü ������
    [SerializeField]
    private Transform spawnPoint;        //�߻�ü ���� ��ġ
    [SerializeField]
    private float attackRate = 0.5f;     //���� �ӵ�
    [SerializeField]
    private float attackRange = 5.0f;    //���� ����
    [SerializeField]
    private int attackDamage = 1; //���ݷ�


    private WeaponState weaponState = WeaponState.SearchTarget; //������ ����
    private Transform attackTarget = null;   //���ݴ��
    private EnemySpawner enemySpawner;

    private Animator animator;


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
        animator = GetComponent<Animator>();
        // Ÿ���� ������ �� ���� �˻��ϵ��� ����
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
            if (dx < 0)
            {
                localScale.x = Mathf.Abs(localScale.x) * -1; // ����
            }
            else
            {
                localScale.x = Mathf.Abs(localScale.x); // ������
            }
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

                    // ���� �˻� ���� ���� ���� �ð��� ���� ������ ���
                    if (enemy.spawnTime < oldestSpawnTime)
                    {
                        oldestSpawnTime = enemy.spawnTime;
                        oldestEnemy = enemy.transform;
                        attackTarget = oldestEnemy;
                    }
                }
            }

            
        }
    }

    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            //1.Ÿ���� �ִ��� �˻�(�ٸ��߻�ü�� ���� ����)
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }
            //2.Ÿ���� ���ݹ��� �ȿ� �ִ��� �˻�(���� ������ ����� ���ο� �� Ž��)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if (distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }

            animator.SetTrigger("attack");
            //4.����(�߻�ü ����)
            SpawnProjectile();
            
            //3.���ݼӵ� �ð� ��ŭ ���
            yield return new WaitForSeconds(attackRate);


        }
    }
    private void SpawnProjectile()
    {
        if (projectilePrefab != null && spawnPoint != null && attackTarget != null)
        {
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            clone.GetComponent<Projectile>().Setup(attackTarget,attackDamage);
           
        }
    }
}
