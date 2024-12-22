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
        // �ڵ����� ã�� ���, �ν����Ϳ��� ������ ����鸸 ���
        if (spells.Count == 0)
        {
            Debug.LogWarning("No spells assigned to SpellManager!");
        }
    }

    public void CastSpell()
    {
        if (spells.Count > 0)
        {
            int Randint = Random.Range(0, spells.Count); //����Ʈ ���� ������ 
            spells[Randint].StartTargeting();
            playerGold.CurrentGold -= spellCost;
            spellCost += 50;
        }
    }

 

}