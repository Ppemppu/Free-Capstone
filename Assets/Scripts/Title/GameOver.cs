using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

    }

    public void Show()
    {
        rect.localScale = Vector3.one;
        Time.timeScale = 0;
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        Time.timeScale = 1;
    }
    
}
