using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    public float damagePerTick ;  // ƽ�� ������
    public float tickRate ;      // ƽ ���� (��)
    public float duration;        // ���� ���ӽð�
    private float tickTimer = 0f;
    private float durationTimer = 0f;

    private void Update()
    {
        // ���ӽð� üũ
        durationTimer += Time.deltaTime;
        if (durationTimer >= duration)
        {
            Destroy(gameObject);
            return;
        }

        // ������ ƽ üũ
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickRate)
        {
            tickTimer = 0f;
            DealDamage();
        }
    }
    private void DealDamage()
    {
        // ���� �ݶ��̴� ũ�⸸ŭ �� ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP enemy = collider.GetComponent<EnemyHP>();
                if (enemy != null)
                {
                    enemy.spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
                    enemy.TakeDamage(damagePerTick);
                }
            }
        }
    }
  
}