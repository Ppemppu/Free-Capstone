using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButton : MonoBehaviour
{
    public SpellManager spellManager;
    public int spellIndex;  // � �������� �����ϱ� ���� �ε���

    public void OnClick()
    {
        //spellManager.CastSpell(spellIndex);
        spellManager.CastSpell();
    }
}