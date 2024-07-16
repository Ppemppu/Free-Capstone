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
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private WaveSystem waveSystem;
  



    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text=playerGold.CurrentGold.ToString();
        textLeftTime.text = string.Format("{0}:{1:00}", (int)waveSystem.LeftTime / 60, (int)waveSystem.LeftTime % 60);
        textcurrentWave.text="WAVE"+waveSystem.CurrentWave.ToString();
    }

}
