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
  
}