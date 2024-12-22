using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    private TextMeshProUGUI textLeftTime;
    [SerializeField]
    private TextMeshProUGUI textcurrentWave;
    [SerializeField]
    private TextMeshProUGUI textcurrentWarriorLevel;
    [SerializeField]
    private TextMeshProUGUI textcurrentWarriorCost;
    [SerializeField]
    private TextMeshProUGUI textcurrentMageLevel;
    [SerializeField]
    private TextMeshProUGUI textcurrentMageCost;
    [SerializeField]
    private TextMeshProUGUI textcurrentArcherLevel;
    [SerializeField]
    private TextMeshProUGUI textcurrentArcherCost;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private WaveSystem waveSystem;
    [SerializeField]
    private TowerUpgradeManager upgradeManager;
  



    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + waveSystem.returnMaxEnemy();

        textPlayerGold.text=playerGold.CurrentGold.ToString()+"G";
        textLeftTime.text = string.Format("{0}:{1:00}", (int)waveSystem.LeftTime / 60, (int)waveSystem.LeftTime % 60);
        textcurrentWave.text="WAVE"+waveSystem.CurrentWave.ToString();
        textcurrentWarriorLevel.text="LV"+upgradeManager.GetUpgradeLevel(TowerType.Warrior).ToString();
        textcurrentMageLevel.text = "LV" + upgradeManager.GetUpgradeLevel(TowerType.Mage).ToString();
        textcurrentArcherLevel.text = "LV" + upgradeManager.GetUpgradeLevel(TowerType.Archer).ToString();
        textcurrentWarriorCost.text = (100 * (int)Mathf.Pow(1.5f, upgradeManager.GetUpgradeLevel(TowerType.Warrior))).ToString()+"G";
        textcurrentMageCost.text = (100 * (int)Mathf.Pow(1.5f, upgradeManager.GetUpgradeLevel(TowerType.Mage))).ToString()+"G";
        textcurrentArcherCost.text = (100 * (int)Mathf.Pow(1.5f, upgradeManager.GetUpgradeLevel(TowerType.Archer))).ToString()+"G";
    }

}
