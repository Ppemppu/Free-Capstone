using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{   
    [SerializeField] private List<SpellBase> spells = new List<SpellBase>();
    [SerializeField] public PlayerGold playerGold;
    private int spellCost=200;

    private void Start()
    {
        // 자동으로 찾는 대신, 인스펙터에서 설정한 스펠들만 사용
        if (spells.Count == 0)
        {
            Debug.LogWarning("No spells assigned to SpellManager!");
        }
    }

    public void CastSpell()
    {
        if (spells.Count > 0)
        {
            int Randint = Random.Range(0, spells.Count); //리스트 안의 랜덤한 
            spells[Randint].StartTargeting();
            playerGold.CurrentGold -= spellCost;
            spellCost += 50;
        }
    }

 

}