using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFieldSpell : SpellBase
{
    public GameObject fieldPrefab;        // ���� ������
    public float damagePerTick = 10f;    // ƽ�� ������
    public float tickRate = 0.5f;        // �������� �ִ� �ֱ�
    public float duration = 5f;          // ���� ���ӽð�

    protected override void CreateRangeIndicator()
    {
        base.CreateRangeIndicator();

        SpriteRenderer spriteRenderer = rangeIndicator.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.3f);
        spriteRenderer.sortingOrder = 10;
    }

    protected override void CastSpell(Vector3 position)
    {
        // ���� ����
        GameObject field = Instantiate(fieldPrefab, position, Quaternion.identity);

        // ���� ũ�� ����
        field.transform.localScale = new Vector3(radius * 2, radius * 2, 1);

        // ���� ������Ʈ ����
        DamageField damageField = field.GetComponent<DamageField>();
        if (damageField != null)
        {
            damageField.damagePerTick = this.damagePerTick;
            damageField.tickRate = this.tickRate;
            damageField.duration = this.duration;
        }
    }
}