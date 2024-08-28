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

        // �ʱ�ȭ
        foreach (TowerType type in Enum.GetValues(typeof(TowerType)))
        {
            upgradeLevel[type] = 1;
        }
    }

    public void UpgradeTowerType(TowerType type)
    {
        int upgradeCost = 100 * (int)Mathf.Pow(1.5f, upgradeLevel[type]);
        Debug.Log(upgradeCost);
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

    public int GetUpgradeLevel(TowerType type)
    {
        return upgradeLevel[type];
    }

}