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

    private Dictionary<TowerRank, float> rankProbabilities = new Dictionary<TowerRank, float> //타워 등급 확률
    {
        { TowerRank.Normal, 0.4f },//40%
        { TowerRank.Rare, 0.3f },//30%
        { TowerRank.Epic, 0.15f },//15%
        { TowerRank.Unique, 0.1f },//10%
        { TowerRank.Legendary, 0.05f }//5%
    };
    public TowerData GetRandomTowerPrefab() //타워 랜덤 설정
    {
        float randomValue = UnityEngine.Random.value;
        float cumulativeProbability = 0.0f;

        foreach (var rank in rankProbabilities.Keys)
        {
            cumulativeProbability += rankProbabilities[rank];
            if (randomValue <= cumulativeProbability)
            {
                var selectedTowers = new List<TowerData>();
                foreach (var tower in AllTowers)
                {
                    if (tower.Rank == rank)
                    {
                        selectedTowers.Add(tower);
                    }
                }

                if (selectedTowers.Count > 0)
                {
                    return selectedTowers[UnityEngine.Random.Range(0, selectedTowers.Count)];
                }
            }
        }

        return null; // 어떤 타워도 선택되지 않았을 경우
    }

    public void UpgradeArchers() => TowerUpgradeManager.Instance.UpgradeTowerType(TowerType.Archer);
    public void UpgradeMages() => TowerUpgradeManager.Instance.UpgradeTowerType(TowerType.Mage);
    public void UpgradeWarrior() => TowerUpgradeManager.Instance.UpgradeTowerType(TowerType.Warrior);

}

