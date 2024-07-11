using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] towerPrefabs = new GameObject[2]; // ���� �׽�Ʈ
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
        // Ÿ�� ���⿡ enemySpawner ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
