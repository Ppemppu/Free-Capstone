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
        // ���콺 ���� ��ư (Ÿ�� ����)
        if (Input.GetMouseButtonDown(0))
        {
            CheckMouseAction(true);
        }
        // ���콺 ������ ��ư (Ÿ�� �Ǹ�)
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