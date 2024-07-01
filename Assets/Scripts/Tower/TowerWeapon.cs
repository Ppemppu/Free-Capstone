using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum WeaponState { SearchTarget=0, AttackToTarget }
public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab; //발사체 프리팹
    [SerializeField]
    private Transform spawnPoint;        //발사체 생성 위치
    [SerializeField]
    private float attackRate = 1f;     //공격 속도
    [SerializeField]
    private float attackRange = 5.0f;    //공격 범위
    private WeaponState weaponState = WeaponState.SearchTarget; //무기의 상태
    private Transform attackTarget = null;   //공격대상
    private EnemySpawner enemySpawner;


    public void Setup(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;

        //최초상태를 Weaponstate.SearchTarget으로 설정
        ChangeState(WeaponState.SearchTarget);
    }

    public void ChangeState(WeaponState newState)
    {
        //이전에 재생중이던 상태 종료
        StopCoroutine(weaponState.ToString());
        //상태 변경
        weaponState = newState;
        //새로운 상태 재생
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
        float dx=attackTarget.position.x-transform.position.x;
        float dy=attackTarget.position.y-transform.position.y;

        float degree =Mathf.Atan2(dy,dx)*Mathf.Rad2Deg;
        transform.rotation=Quaternion.Euler(0,0,degree);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            float oldestSpawnTime = Mathf.Infinity;
            Transform oldestEnemy = null;

            // EnemySpawner의 enemyList 안에 있는 현재 맵에 존재하는 모든 적 검사
            for (int i = 0; i < enemySpawner.EnemyList.Count; i++)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position,transform.position);
                if (distance <= attackRange)
                {
                    Enemy enemy = enemySpawner.EnemyList[i].GetComponent<Enemy>();

                    // 현재 검사 중인 적의 생성 시간이 가장 오래된 경우
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
            //1.타겟이 있는지 검사(다른발사체에 의해 제거)
            if (attackTarget == null)
            {
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }
            //2.타겟이 공격범위 안에 있는지 검사(공격 범위를 벗어나면 새로운 적 탐색)
            float distance = Vector3.Distance(attackTarget.position, transform.position);
            if(distance > attackRange)
            {
                attackTarget = null;
                ChangeState(WeaponState.SearchTarget);
                yield break;
            }

            //3.공격속도 시간 만큼 대기
            yield return new WaitForSeconds(attackRate);

            //4.공격(발사체 생성)
            SpawnProjectile();
           
        }
    }
    private void SpawnProjectile()
    {
        GameObject clone=Instantiate(projectilePrefab,spawnPoint.position, Quaternion.identity);
        clone.GetComponent<Projectile>().Setup(attackTarget);
    }
}

