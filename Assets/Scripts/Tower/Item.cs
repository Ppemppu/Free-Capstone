using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;
    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel =texts[0];
        textName= texts[1];
        textDesc= texts[2];
        textName.text = data.itemName;

    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level);
        textDesc.text = string.Format(data.itemDesc);
    }
   
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Buffer:
                float fixedBonus = data.FixedDamageIncrease.Length > level ? data.FixedDamageIncrease[level] : 0f;
                float percentBonus = data.PercentDamageIncrease.Length > level ? data.PercentDamageIncrease[level] : 0f;

                TowerType towerType = (TowerType)Enum.Parse(typeof(TowerType), data.TowerType);
                TowerUpgradeManager.Instance.UpdateArtifactEffects(towerType, fixedBonus, percentBonus);
                TowerUpgradeManager.Instance.UpdateAllTowers();
                break;
            case ItemData.ItemType.time:
                WaveSystem.Instance.increaseWaveTime(10);
                break;
        }
        level++;

        if (level == data.Effective.Length)
        {
            GetComponent<Button>().interactable = false;
        }

    }
}
