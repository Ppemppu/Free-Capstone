using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerType
{
    Warrior,
    Mage,
    Archer
}
public enum TowerRank
{
    Normal,
    Rare,
    Epic,
    Unique,
    Legendary
}



public class TowerManager : MonoBehaviour
{
    public TowerData[] AllTowers;

    private Dictionary<TowerRank, float> weight = new Dictionary<TowerRank, float> //Ÿ�� ��� Ȯ��
    {
        { TowerRank.Normal, 0.4f },//40%
        { TowerRank.Rare, 0.3f },//30%
        { TowerRank.Epic, 0.15f },//15%
        { TowerRank.Unique, 0.1f },//10%
        { TowerRank.Legendary, 0.05f }//5%
    };
    public TowerData GetRandomTowerPrefab() //Ÿ�� ���� ����
    {
        float randomValue = UnityEngine.Random.value;
        float total = 0.0f; 

        foreach (TowerRank rank in weight.Keys)
        {
            total += weight[rank];
            if (randomValue <= total)
            {
                var selectedTowers = new List<TowerData>();
                foreach (TowerData tower in AllTowers)
                {
                    if (tower.Rank == rank)
                    {
                        selectedTowers.Add(tower);
                    }
                }

                if (selectedTowers.Count > 0)
                {
                    return selectedTowers[UnityEngine.Random.Range(0, selectedTowers.Count)]; //���õ� ����� Ÿ���� �ü�, ���� ,���� ���� ����
                }
            }
        }

        return null; // � Ÿ���� ���õ��� �ʾ��� ���
    }

    public void UpgradeArchers() => TowerUpgradeManager.Instance.UpgradeTowerType(TowerType.Archer);
    public void UpgradeMages() => TowerUpgradeManager.Instance.UpgradeTowerType(TowerType.Mage);
    public void UpgradeWarrior() => TowerUpgradeManager.Instance.UpgradeTowerType(TowerType.Warrior);

}

