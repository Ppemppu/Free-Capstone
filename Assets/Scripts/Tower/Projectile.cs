using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    private bool isSplashDamage;
    private int splashDamage;
    private float splashRadius;

    public void Setup(Transform target,float damage, bool isSplashDamage = false, int splashDamage = 0, float splashRadius = 0f)
    {
        movement2D=GetComponent<Movement2D>();
        this.target = target;  //타워가 설정해준 target
        this.damage = damage; //타워가 설정해준 공격력
        this.isSplashDamage = isSplashDamage; //스플래시 데미지 설정
        this.splashDamage = splashDamage; //스플래시 데미지값
        this.splashRadius = splashRadius; //스플래시 범위
    }

    private void Update()
    {
        if (target != null)
        {
            if (target.position.x < transform.position.x)
            {
                FlipSprite(true); // 이미지 좌우 반전
            }
            else
            {
                FlipSprite(false); // 이미지 원래대로
            }

            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject); // 타겟이 없으면 발사체 삭제
        }
    }

    private void FlipSprite(bool flip)
    {
        Vector3 scale = transform.localScale;

        if (flip)
            scale.x = -Mathf.Abs(scale.x); // 좌우 반전
        else
            scale.x = Mathf.Abs(scale.x); // 원래대로

        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;
        if (collision.transform != target) return;

        // 주 목표에 데미지 적용
        EnemyHP targetEnemyHP = collision.GetComponent<EnemyHP>();
        if (targetEnemyHP != null)
        {
            targetEnemyHP.TakeDamage(damage);
        }

        // 스플래시 데미지 적용
        if (isSplashDamage)
        {
            ApplySplashDamage();
        }

        Destroy(gameObject);
    }
    private void ApplySplashDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, splashRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy") && hitCollider.transform != target)
            {
                EnemyHP enemyHP = hitCollider.GetComponent<EnemyHP>();
                if (enemyHP != null)
                {
                    enemyHP.TakeDamage(splashDamage);
                }
            }
        }
    }
}
