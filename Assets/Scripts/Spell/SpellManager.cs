using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{   
    [SerializeField] private List<SpellBase> spells = new List<SpellBase>();

    private void Start()
    {
        // �ڵ����� ã�� ���, �ν����Ϳ��� ������ ����鸸 ���
        if (spells.Count == 0)
        {
            Debug.LogWarning("No spells assigned to SpellManager!");
        }
    }

    public void CastSpell(int spellIndex)
    {
        if (spellIndex < spells.Count)
        {
            spells[spellIndex].StartTargeting();
        }
    }
    //public void CastSpell()
    //{
    //    if (spells.Count > 0)
    //    {
    //        int Randint = Random.Range(0, spells.Count);
    //        spells[Randint].StartTargeting();
    //    }
    //}

    // ��Ÿ�ӿ��� ���� �߰�
    public void AddSpell(SpellBase spell)
    {
        if (!spells.Contains(spell))
        {
            spells.Add(spell);
        }
    }

    // ��Ÿ�ӿ��� ���� ����
    public void RemoveSpell(SpellBase spell)
    {
        spells.Remove(spell);
    }
}