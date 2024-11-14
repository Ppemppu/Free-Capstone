using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowField : MonoBehaviour
{
    public float duration;        // 장판 지속시간
    private float durationTimer = 0f;
    public float slowAmount = 0.5f;

    private void Update()
    {
        SlowCasting();
        // 지속시간 체크
        durationTimer += Time.deltaTime;
        if (durationTimer >= duration)
        {
            Destroy(gameObject);
            return;
        }
    }

    private void SlowCasting()
    {
        // 장판 콜라이더 크기만큼 적 검출
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplySlow(slowAmount,duration);
                }
            }
        }
    }
}
