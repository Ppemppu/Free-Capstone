using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpell : SpellBase
{
    public float damage = 50f;

    protected override void CastSpell(Vector3 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyHP targetEnemyHP = collider.GetComponent<EnemyHP>();
                if (targetEnemyHP != null)
                {
                    // Enemy 클래스에 TakeDamage 메소드가 있다고 가정
                    targetEnemyHP.TakeDamage(damage);
                }
            }
        }
    }
}