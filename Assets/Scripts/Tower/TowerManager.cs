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

[Serializable]
public class TowerData
{
    public TowerType Type;
    public TowerRank Rank;
    public GameObject Prefab;
}
public class Tower : MonoBehaviour
{
    public TowerData Data;
    public float Damage;
    public float attackRate;
    public float attackRange;
    public void Upgrade()
    {
        Damage += 100f;

        // TowerWeapon 스크립트에 접근하여 스탯을 업데이트
        TowerWeapon weapon = GetComponent<TowerWeapon>();
        if (weapon != null)
        {
            weapon.UpdateStats(Damage, attackRate);
        }
    }
}


public class TowerManager : MonoBehaviour
{
    public TowerData[] AllTowers;

    private Dictionary<TowerRank, float> rankProbabilities = new Dictionary<TowerRank, float> //타워 등급 확률
    {
        { TowerRank.Normal, 0.4f },
        { TowerRank.Rare, 0.3f },
        { TowerRank.Epic, 0.2f },
        { TowerRank.Unique, 0.08f },
        { TowerRank.Legendary, 0.02f }
    };
    public GameObject GetRandomTowerPrefab()
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
                    return selectedTowers[UnityEngine.Random.Range(0, selectedTowers.Count)].Prefab;
                }
            }
        }

        return null; // 어떤 타워도 선택되지 않았을 경우
    }

    public void UpgradeTowersByType(TowerType type)
    {
        foreach (var towerObject in FindObjectsOfType<Tower>())
        {
            if (towerObject.Data.Type == type)
            {
                towerObject.Upgrade();
            }
        }
    }
    public GameObject GetTowerPrefab(TowerType type, TowerRank rank)
    {
        foreach (var tower in AllTowers)
        {
            if (tower.Type == type && tower.Rank == rank)
            {
                return tower.Prefab;
            }
        }
        return null;
    }
}
