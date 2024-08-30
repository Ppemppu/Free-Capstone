using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public GameObject gameover_Panel;

    [SerializeField]
    private float maxHP =50;
    private float currentHP;
    private EnemySpawner enemySpawner;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    public void Awake()
    {
        gameover_Panel.SetActive(false);
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
    public float GetHP()
    {
        return currentHP;
    }
    public void GameOver()
    {
        gameover_Panel.SetActive(true);
        //SceneManager.LoadScene("GameOver");
    }

}
