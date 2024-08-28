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
        int upgradeCost = 100 * (int)Mathf.Pow(1.5f, upgradeLevel[type]);
        Debug.Log(upgradeCost);
        // 현재 소유한 골드가 강화 비용보다 많은지 확인
        if (playerGold.CurrentGold >= upgradeCost)
        {
            // 골드 차감 및 강화 레벨 증가
            playerGold.CurrentGold -= upgradeCost;
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
        else
        {
            // 골드가 부족한 경우 처리 (예: 사용자에게 알림)
            Debug.Log("Not enough gold to upgrade!");
        }
    }

    public int GetUpgradeLevel(TowerType type)
    {
        return upgradeLevel[type];
    }

}