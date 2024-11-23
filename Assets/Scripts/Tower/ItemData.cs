using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Buffer, special }
    [Header("#Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    public string itemDesc;
    public Sprite itemIcon;

    [Header("#Level Data")]
    public float [] Effective; //강화 효과 수치
    public string TowerType;  //타워 종류
    public bool percent; //고정 퍼센트 여부
    public float[] FixedDamageIncrease; // 고정 데미지 증가
    public float[] PercentDamageIncrease; // 퍼센트 데미지 증가
}
