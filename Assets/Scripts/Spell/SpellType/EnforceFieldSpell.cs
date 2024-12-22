using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceFieldSpell : SpellBase
{
    public GameObject fieldPrefab;        // ���� ������
    public float damagePerTick = 10f;    // ƽ�� ������
    public float tickRate = 0.5f;        // �������� �ִ� �ֱ�
    public float duration = 5f;          // ���� ���ӽð�

    protected override void CreateRangeIndicator()
    {
        base.CreateRangeIndicator();
        SpriteRenderer spriteRenderer = rangeIndicator.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;
    }

    protected override void CastSpell(Vector3 position)
    {
        // ���� ����
        GameObject field = Instantiate(fieldPrefab, position, Quaternion.identity);

        // ���� ũ�� ����
        field.transform.localScale = new Vector3(radius * 4, radius * 4, 1);
        EnforceField enforceField = field.GetComponent<EnforceField>();
        if (field != null)
        {
            enforceField.duration = this.duration;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, radius);


        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Tower"))
            {
                TowerWeapon towerWeapon = collider.GetComponent<TowerWeapon>();
                if (towerWeapon != null)
                {
                    towerWeapon.EnforceSpell(duration);
                }
            }
        }


    }
}
