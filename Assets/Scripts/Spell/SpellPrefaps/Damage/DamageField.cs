using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    public float damagePerTick ;  // 틱당 데미지
    public float tickRate ;      // 틱 간격 (초)
    public float duration;        // 장판 지속시간
    private float tickTimer = 0f;
    private float durationTimer = 0f;

    private void Update()
    {
        // 지속시간 체크
        durationTimer += Time.deltaTime;
        if (durationTimer >= duration)
        {
            Destroy(gameObject);
            return;
        }

        // 데미지 틱 체크
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickRate)
        {
            tickTimer = 0f;
            DealDamage();
        }
    }
    private void DealDamage()
    {
        // 장판 콜라이더 크기만큼 적 검출
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