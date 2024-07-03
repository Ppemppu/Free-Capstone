using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private int damage;

    public void Setup(Transform target,int damage)
    {
        movement2D=GetComponent<Movement2D>();
        this.target = target;  //타워가 설정해준 target
        this.damage = damage; //타워가 설정해준 공격력
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction=(target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != target) return;

        collision.GetComponent<EnemyHP>().TakeDamage(damage);//적 체력을 damage만큼 감소
        Destroy(gameObject); //발사체 오브젝트 삭제
    }

}
