using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner; //현재 맵에 존재하는 적 리스트 정보를 얻기 위해
    
    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)
        {
            return;
        }
        tile.IsBuildTower = true;
        //선택한타일의 위치에 타워 건설
        GameObject clone= Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //타워 무기에 enemySpanwer 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
