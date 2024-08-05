using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] towerPrefabs = new GameObject[0]; // ���� �׽�Ʈ
    [SerializeField]
    private EnemySpawner enemySpawner; // ���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����
    [SerializeField]
    private int towerBuildGold = 50; // Ÿ�� �Ǽ��� ���Ǵ� ���
    [SerializeField]
    private PlayerGold playerGold;

    public void SpawnTower(Transform tileTransform)
    {
        if (towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        // �̹� Ÿ���� ��ġ�� Ÿ���̸� ��
        if (tile.IsBuildTower == true)
        {
            return;
        }
        tile.IsBuildTower = true;
        playerGold.CurrentGold -= towerBuildGold;

        // ���� �׽�Ʈ
        int randomIndex = Random.Range(0, towerPrefabs.Length);
        GameObject selectedTowerPrefab = towerPrefabs[randomIndex];


        // ������ Ÿ���� ��ġ�� Ÿ�� �Ǽ�
        GameObject clone = Instantiate(selectedTowerPrefab, tileTransform.position, Quaternion.identity);
        clone.transform.SetParent(tileTransform); // Ÿ���� Ÿ���� �ڽ����� ����
        // Ÿ�� ���⿡ enemySpawner ���� ����

        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }

    public void SellTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();
        if (tile.IsBuildTower == true)
        {
            GameObject towerObject = FindTowerOnTile(tileTransform);
            if (towerObject != null)
            {
                int sellPrice = (int)(towerBuildGold * 0.3f); // 30% ȯ��
                playerGold.CurrentGold += sellPrice;

                Destroy(towerObject);
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
    private GameObject FindTowerOnTile(Transform tileTransform)
    {
        foreach (Transform child in tileTransform)
        {
            foreach (GameObject prefab in towerPrefabs)
            {
                if (child.gameObject.name.Contains(prefab.name))
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }
}
