using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private EnemySpawner enemySpawner; //���� �ʿ� �����ϴ� �� ����Ʈ ������ ��� ����
    
    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        if (tile.IsBuildTower == true)
        {
            return;
        }
        tile.IsBuildTower = true;
        //������Ÿ���� ��ġ�� Ÿ�� �Ǽ�
        GameObject clone= Instantiate(towerPrefab, tileTransform.position, Quaternion.identity);
        //Ÿ�� ���⿡ enemySpanwer ���� ����
        clone.GetComponent<TowerWeapon>().Setup(enemySpawner);
    }
}
