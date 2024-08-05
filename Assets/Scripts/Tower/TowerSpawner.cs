using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] towerPrefabs = new GameObject[0]; // 랜덤 테스트
    [SerializeField]
    private EnemySpawner enemySpawner; // 현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    [SerializeField]
    private int towerBuildGold = 50; // 타워 건설에 사용되는 골드
    [SerializeField]
    private PlayerGold playerGold;

    public void SpawnTower(Transform tileTransform)
    {
        if (towerBuildGold > playerGold.CurrentGold)
        {
            return;
        }
        Tile tile = tileTransform.GetComponent<Tile>();

        // 이미 타워가 설치된 타일이면 ㄴ
        if (tile.IsBuildTower == true)
        {
            return;
        }
        tile.IsBuildTower = true;
        playerGold.CurrentGold -= towerBuildGold;

        // 랜덤 테스트
        int randomIndex = Random.Range(0, towerPrefabs.Length);
        GameObject selectedTowerPrefab = towerPrefabs[randomIndex];


        // 선택한 타일의 위치에 타워 건설
        GameObject clone = Instantiate(selectedTowerPrefab, tileTransform.position, Quaternion.identity);
        clone.transform.SetParent(tileTransform); // 타워를 타일의 자식으로 설정
        // 타워 무기에 enemySpawner 정보 전달

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
                int sellPrice = (int)(towerBuildGold * 0.3f); // 30% 환불
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
