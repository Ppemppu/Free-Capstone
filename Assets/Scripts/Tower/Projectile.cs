using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private int damage;
    private bool isSplashDamage;
    private int splashDamage;
    private float splashRadius;

    public void Setup(Transform target,int damage, bool isSplashDamage = false, int splashDamage = 0, float splashRadius = 0f)
    {
        movement2D=GetComponent<Movement2D>();
        this.target = target;  //Ÿ���� �������� target
        this.damage = damage; //Ÿ���� �������� ���ݷ�
        this.isSplashDamage = isSplashDamage; //���÷��� ������ ����
        this.splashDamage = splashDamage; //���÷��� ��������
        this.splashRadius = splashRadius; //���÷��� ����
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

        // �� ��ǥ�� ������ ����
        EnemyHP targetEnemyHP = collision.GetComponent<EnemyHP>();
        if (targetEnemyHP != null)
        {
            targetEnemyHP.TakeDamage(damage);
        }

        // ���÷��� ������ ����
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
