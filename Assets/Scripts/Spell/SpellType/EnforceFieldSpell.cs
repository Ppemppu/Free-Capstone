using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceFieldSpell : SpellBase
{
    public GameObject fieldPrefab;        // 장판 프리팹
    public float damagePerTick = 10f;    // 틱당 데미지
    public float tickRate = 0.5f;        // 데미지를 주는 주기
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
