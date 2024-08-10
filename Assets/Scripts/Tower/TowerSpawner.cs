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

        GameObject towerPrefab = towerManager.GetRandomTowerPrefab();
        if (towerPrefab != null)
        {
            tile.IsBuildTower = true;
            playerGold.CurrentGold -= towerBuildGold;

          

            // 선택한 타일의 위치에 타워 건설
            GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
            clone.transform.SetParent(tileTransform); // 타워를 타일의 자식으로 설정
                                                      // 타워 무기에 enemySpawner 정보 전달

            Tower towerComponent = clone.GetComponent<Tower>();
            clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
        }
    }

    public void SellTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.IsBuildTower)
        {
            Tower tower = tileTransform.GetComponentInChildren<Tower>();
            if (tower != null)
            {
                int sellPrice = (int)(towerBuildGold * 0.3f);  // 30% 환불
                playerGold.CurrentGold += sellPrice;

                Destroy(tower.gameObject);
                tile.IsBuildTower = false;

                Debug.Log($"Tower sold for {sellPrice} gold.");
            }
            else
            {
                Debug.LogWarning("No tower found on this tile.");
            }
        }
        else
        {
            Debug.LogWarning("No tower to sell on this tile.");
        }
    }
    public void UpgradeWarriorTowers()
    {
        towerManager.UpgradeTowersByType(TowerType.Warrior);
        playerGold.CurrentGold -= 1;

    }

    public void UpgradeMageTowers()
    {
        towerManager.UpgradeTowersByType(TowerType.Mage);
    }

    public void UpgradeArcherTowers()
    {
        towerManager.UpgradeTowersByType(TowerType.Archer);
    }
}
