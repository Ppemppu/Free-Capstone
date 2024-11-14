using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFieldSpell : SpellBase
{
    public GameObject fieldPrefab;        // 장판 프리팹
    public float damagePerTick = 10f;    // 틱당 데미지
    public float tickRate = 0.5f;        // 데미지를 주는 주기
    public float duration = 5f;          // 장판 지속시간

    protected override void CreateRangeIndicator()
    {
        base.CreateRangeIndicator();

        SpriteRenderer spriteRenderer = rangeIndicator.GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(1f, 0f, 0f, 0.3f);
        spriteRenderer.sortingOrder = 10;
    }

    protected override void CastSpell(Vector3 position)
    {
        // 장판 생성
        GameObject field = Instantiate(fieldPrefab, position, Quaternion.identity);

        // 장판 크기 설정
        field.transform.localScale = new Vector3(radius * 2, radius * 2, 1);

        // 장판 컴포넌트 설정
        DamageField damageField = field.GetComponent<DamageField>();
        if (damageField != null)
        {
            damageField.damagePerTick = this.damagePerTick;
            damageField.tickRate = this.tickRate;
            damageField.duration = this.duration;
        }
    }
}