using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellManager : MonoBehaviour
{   
    [SerializeField] private List<SpellBase> spells = new List<SpellBase>();

    private void Start()
    {
        // 자동으로 찾는 대신, 인스펙터에서 설정한 스펠들만 사용
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

    // 런타임에서 스펠 추가
    public void AddSpell(SpellBase spell)
    {
        if (!spells.Contains(spell))
        {
            spells.Add(spell);
        }
    }

    // 런타임에서 스펠 제거
    public void RemoveSpell(SpellBase spell)
    {
        spells.Remove(spell);
    }
}