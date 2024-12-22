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

        TowerData towerPrefab = towerManager.GetRandomTowerPrefab();
        if (towerPrefab != null)
        {
            tile.IsBuildTower = true;
            playerGold.CurrentGold -= towerBuildGold;

            // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
            GameObject clone = Instantiate(towerPrefab.Prefab, tileTransform.position, Quaternion.identity);
            clone.transform.SetParent(tileTransform); // Ÿ���� Ÿ���� �ڽ����� ����

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
                int sellPrice = (int)(towerBuildGold * 0.3f);  // 30% ȯ��
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
