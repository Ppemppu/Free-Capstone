using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager Instance;

    private Dictionary<TowerType, int> upgradeLevel = new Dictionary<TowerType, int>();

    [SerializeField]
    private PlayerGold playerGold;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // 초기화
        foreach (TowerType type in Enum.GetValues(typeof(TowerType)))
        {
            upgradeLevel[type] = 1;
        }
    }

    public void UpgradeTowerType(TowerType type)
    {
        playerGold.CurrentGold -= 1;
        upgradeLevel[type]++;
        Debug.Log(upgradeLevel[type]);
        // 기존 타워 강화
        GameObject[] allTowers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject towerObject in allTowers)
        {
            Tower tower = towerObject.GetComponent<Tower>();
            
            if (tower != null && tower.Data.Type == type)
            {
                tower.GetComponent<TowerWeapon>().ApplyUpgrades();
            }
        }
    }

    public int GetUpgradeLevel(TowerType type)
    {
        return upgradeLevel[type];
    }

}