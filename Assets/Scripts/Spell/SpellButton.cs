using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButton : MonoBehaviour
{
    public SpellManager spellManager;
    public int spellIndex;  // 어떤 스펠인지 구분하기 위한 인덱스

    public void OnClick()
    {
        //spellManager.CastSpell(spellIndex);
        spellManager.CastSpell();
    }
}