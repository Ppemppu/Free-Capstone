using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner enemySpawner; // 현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    [SerializeField]
    private int towerBuildGold = 50; // 타워 건설에 사용되는 골드
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private TowerManager towerManager;

    public void SpawnTower(Transform tileTransform)
    {
        if (towerBuildGold > playerGold.CurrentGold)//돈이 부족할 경우
        {
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)//이미 설치된 타일의 경우
        {
            return;
        }

        TowerData towerPrefab = towerManager.GetRandomTowerPrefab();
        if (towerPrefab != null)
        {
            tile.IsBuildTower = true;
            playerGold.CurrentGold -= towerBuildGold;

            // 선택한 타일의 위치에 타워 건설
            GameObject clone = Instantiate(towerPrefab.Prefab, tileTransform.position, Quaternion.identity);
            clone.transform.SetParent(tileTransform); // 타워를 타일의 자식으로 설정

            Tower towerComponent = new Tower();
            TowerWeapon weaponComponent =clone.GetComponent<TowerWeapon>();
            weaponComponent.Setup(enemySpawner);
        }
    }


    public void SellTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.IsBuildTower)
        {
            GameObject towerObject = FindTowerOnTile(tileTransform);
            if (towerObject != null)
            {
                int sellPrice = (int)(towerBuildGold * 0.3f);  // 30% 환불
                playerGold.CurrentGold += sellPrice;

                Destroy(towerObject);
                tile.IsBuildTower = false;
            }   
        }
    }

    private GameObject FindTowerOnTile(Transform tileTransform)
    {
        foreach (Transform child in tileTransform)
        {
            foreach (var towerData in towerManager.AllTowers)
            {
                if (child.gameObject.name.Contains(towerData.Prefab.name))
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }
}
