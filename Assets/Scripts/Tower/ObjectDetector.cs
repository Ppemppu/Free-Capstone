using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;


    private bool flag = true;
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼 (타워 생성)
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseAction(true);
        }
        // 마우스 오른쪽 버튼 (타워 판매)
        else if (Input.GetMouseButtonDown(1))
        {
            CheckMouseAction(false);
        }
    }

    private void CheckMouseAction(bool isSpawn)
    {
        if (flag)
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    if (isSpawn)
                    {
                        towerSpawner.SpawnTower(hit.transform);
                    }
                    else
                    {
                        towerSpawner.SellTower(hit.transform);
                    }
                }
            }
        }
    }
    public void setFlag(bool sflag)
    {
        flag = sflag;
    }
}