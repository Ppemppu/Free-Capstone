using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgradeManager : MonoBehaviour
{
    public static TowerUpgradeManager Instance;

    private Dictionary<TowerType, int> upgradeLevel = new Dictionary<TowerType, int>();
    private Dictionary<TowerType, float> artifactFixedDamage = new Dictionary<TowerType, float>();
    private Dictionary<TowerType, float> artifactPercentDamage = new Dictionary<TowerType, float>();

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
            artifactFixedDamage[type] = 0f;
            artifactPercentDamage[type] = 0f;
        }
    }
    public void UpdateArtifactEffects(TowerType towerType, float fixedIncrease, float percentIncrease)
    {
        artifactFixedDamage[towerType] += fixedIncrease;
        artifactPercentDamage[towerType] += percentIncrease;
    }

    public (float fixedDamage, float percentDamage) GetArtifactEffects(TowerType towerType)
    {
        return (artifactFixedDamage[towerType], artifactPercentDamage[towerType]);
    }


    public void UpgradeTowerType(TowerType type)
    {
        int upgradeCost = 100 * (int)Mathf.Pow(1.5f, upgradeLevel[type]);
        
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
    public void UpdateAllTowers()
    {
        GameObject[] allTowers = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject towerObject in allTowers)
        {
            TowerWeapon weapon = towerObject.GetComponent<TowerWeapon>();
            if (weapon != null)
            {
                weapon.ApplyUpgrades(); // 유물 효과를 포함해 다시 계산
            }
        }
    }

    public int GetUpgradeLevel(TowerType type)
    {
        return upgradeLevel[type];
    }

}