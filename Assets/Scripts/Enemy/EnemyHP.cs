using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP; //�ִ� ü��
    private float currentHP; //���� ü��
    private bool isDie = false; //���� ����ϸ� true
    private Enemy enemy;
    public SpriteRenderer spriteRenderer;
    private Color originalColor;
    
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(float damage)
    {
        //���� ü���� damage��ŭ �����ؼ� ���� ��Ȳ �� �� ���� Ÿ���� ������ ���ÿ� ������ enemy.onDie()�Լ��� ������ ����� �� �ִ�.
        //���� ���� ���°� ��� �����̸� �Ʒ� �ڵ带 �������� �ʴ´�.
        if (isDie == true) return;

        currentHP -= damage;
        Debug.Log($"Enemy took {damage} damage. Current HP: {currentHP}");
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {
            isDie = true;
            //�� ĳ���� ���
            enemy.OnDie();
        }
    }
    public void SetMaxHP(float newMaxHP)
    {
        maxHP = newMaxHP;
        currentHP = maxHP;
    }
    public void Initialize()
    {
        currentHP = maxHP;
        isDie = false;
    }
    public float GetMaxHP()
    {
        return maxHP;
    }


    private IEnumerator HitAlphaAnimation()
    {
        //���� ������ 40%�� ����
        originalColor.a = 0.4f;
        if (spriteRenderer.color != Color.blue)
        {
            spriteRenderer.color = originalColor;
        }
        //0.05�� ���
        yield return new WaitForSeconds(0.1f);
        //���� ������ 100%�� ����
        originalColor.a = 1.0f;
        if(spriteRenderer.color != Color.blue)
        {
            spriteRenderer.color = originalColor;
        }

        // ���� �ڵ�
        ////���� ������ color������ ����
        //Color color = spriteRenderer.color;

        ////���� ������ 40%�� ����
        //color.a = 0.4f;
        //spriteRenderer.color = color;
        ////0.05�� ���
        //yield return new WaitForSeconds(0.1f);
        ////���� ������ 100%�� ����
        //color.a = 1.0f;
        //spriteRenderer.color = color;
    }
}
