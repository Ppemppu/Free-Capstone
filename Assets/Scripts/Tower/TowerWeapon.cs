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
    private float attackRate = 1f;     //���� �ӵ�
    [SerializeField]
    private float attackRange = 5.0f;    //���� ����
    private WeaponState weaponState = WeaponState.SearchTarget; //������ ����
    private Transform attackTarget = null;   //���ݴ��
    private EnemySpawner enemySpawner;


    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        //���ʻ��¸� Weaponstate.SearchTarget���� ����
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //������ ������̴� ���� ����
        StopCoroutine(weaponState.ToString());
        //���� ����
        weaponState = newState;
        //���ο� ���� ���
        StartCoroutine(newState.ToString());
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
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
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

            if (attackTarget != null)
            {
                ChangeState(WeaponState.AttackToTarget);
            }
            yield return null;
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

            //3.���ݼӵ� �ð� ��ŭ ���
            yield return new WaitForSeconds(attackRate);

            //4.����(�߻�ü ����)
            SpawnProjectile();

            if (SpawnProjectile())
            {
                yield return new WaitForSeconds(attackRate);
            }

        }
    }
    private bool SpawnProjectile()
    {
        if (projectilePrefab != null && spawnPoint != null && attackTarget != null)
        {
            GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
            clone.GetComponent<Projectile>().Setup(attackTarget);
            return true;
        }
        return false;
    }
}