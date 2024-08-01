using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP; //최대 체력
    private float currentHP; //현재 체력
    private bool isDie = false; //적이 사망하면 true
    private Enemy enemy;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        //적의 체력이 damage만큼 감소해서 죽을 상황 일 때 여러 타워의 공격을 동시에 받으면 enemy.onDie()함수가 여러번 실행될 수 있다.

        //현재 적의 상태가 사망 상태이면 아래 코드를 실행하지 않는다.
        if (isDie == true) return;
      
        currentHP -= damage;
        Debug.Log($"Enemy took {damage} damage. Current HP: {currentHP}");
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {
            isDie = true;
            //적 캐릭터 사망
            enemy.OnDie();
        }
    }
    private IEnumerator HitAlphaAnimation()
    {
        //현재 색상을 color변수에 저장
        Color color = spriteRenderer.color;

        //적의 투명도를 40%로 설정
        color.a = 0.4f;
        spriteRenderer.color = color;
        //0.05초 대기
        yield return new WaitForSeconds(0.05f);
        //적의 투명도를 100%로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;
    }
}
