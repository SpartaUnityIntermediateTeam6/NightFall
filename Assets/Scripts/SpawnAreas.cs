using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAreas : MonoBehaviour
{
    [SerializeField] List<Rect> spawnAreas;

    [SerializeField] private List<GameObject> spawnPrefabs;

    private bool _spwanPeriod; // 스폰주기 확인

    [SerializeField] private bool enemyPeriod; // 적 스폰 확인

    [SerializeField] private float morningPeriod;

    private float _nowTime;

    Color[] gizmoColor;

    private void Update()
    {
        _nowTime += Time.deltaTime;

        if (_nowTime >= morningPeriod)
        {
            _spwanPeriod = false;
        }
        else if(_nowTime >= 0f && _nowTime < morningPeriod)
        {
            _spwanPeriod = true;
        }

        if (!_spwanPeriod && enemyPeriod)
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnRandom(0);
            }
            _nowTime = -morningPeriod;
            enemyPeriod = false;
        }
        else if (_spwanPeriod && !enemyPeriod)
        {
            for (int i = 0; i < 10; i++)
            {
                SpawnRandom(1);
            }
            enemyPeriod = true;
        }
    }

    void SpawnRandom(int spawnkind)
    {
        if (spawnPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("Enemy Prefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        GameObject randomPrefab = spawnPrefabs[spawnkind];

        Rect randomArea = spawnAreas[spawnkind];

        Vector3 randomPosition = new Vector3(
            Random.Range(randomArea.xMin, randomArea.xMax), 0,
            Random.Range(randomArea.yMin, randomArea.yMax));

        GameObject spawnEnemy = Instantiate(randomPrefab, new Vector3(randomPosition.x, spawnkind, randomPosition.z), Quaternion.identity);

        spawnEnemy.transform.parent = transform;
    }

    



    private void OnDrawGizmosSelected()
    {
        gizmoColor = new Color[3] { new Color(0, 1, 0, .3f), new Color(1, 0, 0, .3f), new Color(0, 1, 0, .3f) };

        int spawnAreasNum = 0;

        if (spawnAreas == null) return;

        foreach (var area in spawnAreas)
        {
            Gizmos.color = gizmoColor[spawnAreasNum];
            Vector3 center = new Vector3(area.x + area.width / 2, 0, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, 1, area.height);

            Gizmos.DrawCube(center, size);
            spawnAreasNum++;
        }
    }
}
