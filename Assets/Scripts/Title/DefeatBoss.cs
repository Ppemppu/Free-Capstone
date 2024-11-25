using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DefeatBoss : MonoBehaviour
{
    RectTransform rect;
    Item[] items;
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items= GetComponentsInChildren<Item>(true);
    }
    public void Show()
    {
        Time.timeScale = 0;
        Next();
        rect.localScale = Vector3.one;
        
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        Time.timeScale = 1;
        WaveSystem.Instance.CompleteChoice();
    }
    void Next()
    {
        //1 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        //2 그중에서 랜덤 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = 2;
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }
        for (int index =0; index<ran.Length; index++)
        {
            Item ranItem = items[ran[index]];
            //3. 만렙아이템의 경우 소비 아이템으로 대체
            if (ranItem.level == ranItem.data.Effective.Length)
                items[4].gameObject.SetActive(true);  
            else
            ranItem.gameObject.SetActive(true);
        }
    }
}
