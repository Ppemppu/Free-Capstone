using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private TowerWeapon weapon;

    private void Awake()
    {
        weapon = GetComponent<TowerWeapon>();
    }
    public void Initialize(TowerData data)
    {
        Data = data;
        Upgrade(); // 초기 생성 시 현재 업그레이드 레벨 적용
    }
    public void Upgrade()
    {
        if (weapon != null)
        {
            weapon.ApplyUpgrades();
        }
    }
}