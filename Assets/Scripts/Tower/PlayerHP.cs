using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHP : MonoBehaviour
{
    public GameObject gameover_Panel;

    [SerializeField]
    private float maxHP;
    private float currentHP;
    private EnemySpawner enemySpawner;
    public GameOver gameOverUI;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    public void Awake()
    {
        currentHP =0; 
    }

    public void ChangeHP(int amount)
    {
        currentHP += amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP); // HP가 0에서 maxHP 사이에 있도록 제한
    }
    public float GetHP()
    {
        return currentHP;
    }
    public void resetHP()
    {
        currentHP = 0;
    }
    public void GameOver()
    {
        gameOverUI.Show();
    }

}
