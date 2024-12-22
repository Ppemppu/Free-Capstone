using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSpell : SpellBase
{
    public float slowAmount = 0.5f;
    public float slowDuration = 3f;

    protected override void CastSpell(Vector3 position)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.ApplySlow(slowAmount, slowDuration);
                }
            }
        }

    }
}
