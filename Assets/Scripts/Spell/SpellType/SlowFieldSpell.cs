using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFieldSpell : SpellBase
{
    public GameObject fieldPrefab;        // ���� ������
    public float slowAmount = 0.5f;
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
        field.transform.localScale = new Vector3(radius * 2, radius * 2, 1);

        // ���� ������Ʈ ����
        SlowField slowField = field.GetComponent<SlowField>();
        if (slowField != null)
        {
            slowField.slowAmount = this.slowAmount;
            slowField.duration = this.duration;
        }
    }
}
