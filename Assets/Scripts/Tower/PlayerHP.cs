using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private float maxHP =100;
    private float currentHP;
    private EnemySpawner enemySpawner;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP =0; 
    }

    public void ChangeHP(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HP�� 0���� maxHP ���̿� �ֵ��� ����

        if (currentHP >= maxHP)
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}
