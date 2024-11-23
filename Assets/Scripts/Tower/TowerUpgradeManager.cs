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

        // �ʱ�ȭ
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
        
        // ���� ������ ��尡 ��ȭ ��뺸�� ������ Ȯ��
        if (playerGold.CurrentGold >= upgradeCost)
        {
            // ��� ���� �� ��ȭ ���� ����
            playerGold.CurrentGold -= upgradeCost;
            upgradeLevel[type]++;
            Debug.Log(upgradeLevel[type]);

            // ���� Ÿ�� ��ȭ
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
            // ��尡 ������ ��� ó�� (��: ����ڿ��� �˸�)
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
                weapon.ApplyUpgrades(); // ���� ȿ���� ������ �ٽ� ���
            }
        }
    }

    public int GetUpgradeLevel(TowerType type)
    {
        return upgradeLevel[type];
    }

}