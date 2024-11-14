using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFieldSpell : SpellBase
{
    public GameObject fieldPrefab;        // 장판 프리팹
    public float slowAmount = 0.5f;
    public float duration = 5f;          // 장판 지속시간

    protected override void CreateRangeIndicator()
    {
        base.CreateRangeIndicator();
        SpriteRenderer spriteRenderer = rangeIndicator.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;
    }

    protected override void CastSpell(Vector3 position)
    {
        // 장판 생성
        GameObject field = Instantiate(fieldPrefab, position, Quaternion.identity);

        // 장판 크기 설정
        field.transform.localScale = new Vector3(radius * 2, radius * 2, 1);

        // 장판 컴포넌트 설정
        SlowField slowField = field.GetComponent<SlowField>();
        if (slowField != null)
        {
            slowField.slowAmount = this.slowAmount;
            slowField.duration = this.duration;
        }
    }
}
