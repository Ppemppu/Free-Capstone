using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����
    [SerializeField]
    private int towerBuildGold = 50; // Ÿ�� �Ǽ��� ���Ǵ� ���
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private TowerManager towerManager;

    public void SpawnTower(Transform tileTransform)
    {
        if (towerBuildGold > playerGold.CurrentGold)//���� ������ ���
        {
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)//�̹� ��ġ�� Ÿ���� ���
        {
            return;
        }

        GameObject towerPrefab = towerManager.GetRandomTowerPrefab();
        if (towerPrefab != null)
        {
            tile.IsBuildTower = true;
            playerGold.CurrentGold -= towerBuildGold;

          

            // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
            GameObject clone = Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
            clone.transform.SetParent(tileTransform); // Ÿ���� Ÿ���� �ڽ����� ����
                                                      // Ÿ�� ���⿡ enemySpawner ���� ����

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
                int sellPrice = (int)(towerBuildGold * 0.3f);  // 30% ȯ��
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
