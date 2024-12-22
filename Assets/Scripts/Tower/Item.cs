using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Unity.PlasticSCM.Editor.WebApi.CredentialsResponse;

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
                float fixedBonus = data.FixedDamageIncrease.Length > level ? data.FixedDamageIncrease[level] : 0f;  //오류 방지 삼항연산자
                float percentBonus = data.PercentDamageIncrease.Length > level ? data.PercentDamageIncrease[level] : 0f;

                if (data.TowerType == "All")
                {
                    TowerUpgradeManager.Instance.UpdateArtifactEffects(TowerType.Warrior, fixedBonus, percentBonus);
                    TowerUpgradeManager.Instance.UpdateAllTowers();
                    TowerUpgradeManager.Instance.UpdateArtifactEffects(TowerType.Archer, fixedBonus, percentBonus);
                    TowerUpgradeManager.Instance.UpdateAllTowers();
                    TowerUpgradeManager.Instance.UpdateArtifactEffects(TowerType.Mage, fixedBonus, percentBonus);
                    TowerUpgradeManager.Instance.UpdateAllTowers();
                }
                else
                {
                    TowerType towerType = (TowerType)Enum.Parse(typeof(TowerType), data.TowerType);
                    TowerUpgradeManager.Instance.UpdateArtifactEffects(towerType, fixedBonus, percentBonus);
                    TowerUpgradeManager.Instance.UpdateAllTowers();
                }
                break;

            case ItemData.ItemType.time:

                float increase = data.Effective.Length > level ? data.Effective[level] : 0f;
                if (data.TowerType == "0")
                {
                    
                    WaveSystem.Instance.increaseWaveTime(increase);
                }
                else
                {
                    PlayerGold.Instance.CurrentGold += (int)Math.Ceiling(increase);
                }
                    
                break;
        }
        level++;


        if (level == data.Effective.Length)
        {
            GetComponent<Button>().interactable = false;
        }

    }
}
