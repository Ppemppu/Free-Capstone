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
                    // Enemy Ŭ������ TakeDamage �޼ҵ尡 �ִٰ� ����
                    targetEnemyHP.TakeDamage(damage);
                }
            }
        }
    }
}