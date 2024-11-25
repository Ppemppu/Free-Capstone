using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Item", menuName ="Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Buffer, time }
    [Header("#Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("#Level Data")]
    public float [] Effective; //��ȭ ȿ�� ��ġ
    public string TowerType;  //Ÿ�� ����
    public bool percent; //���� �ۼ�Ʈ ����
    public float[] FixedDamageIncrease; // ���� ������ ����
    public float[] PercentDamageIncrease; // �ۼ�Ʈ ������ ����
}