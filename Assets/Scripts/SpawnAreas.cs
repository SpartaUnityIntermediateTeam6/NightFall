using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnAreas : MonoBehaviour
{
    public enum SpawnPrefabType
    {
        Morning,
        Night,
        Always
    }

    [SerializeField] List<Rect> spawnAreas;  // 스폰 지역

    [SerializeField] private List<GameObject> enemyPrefabs;  // 적 프리팹

    [SerializeField] private List<GameObject> resourcePrefabs; // 자원 프리팹

    [SerializeField] private List<GameObject> alwaysPrefabs;  // 항상 나오는 프리팹

    [SerializeField] private float spawnDelay;  // 동시 생성 방지, 스폰 딜레이

    [SerializeField] private float morningPeriod;  // 밤낮 주기

    [SerializeField] private float alwaysPeriod;  // 항상 나오는 오브젝트 주기

    [SerializeField] private int spawnCount;  // 생성 갯수

    private float realMorningPeriod;

    private float _alwaysSpawnTime = 0f;  // always프리팹 스폰 시간

    private bool _spawnPeriod; // 스폰주기 확인

    private float _nowTime;

    Color[] gizmoColor;

    private Coroutine _coroutine;
    private Coroutine _alwaysCoroutine;

    private void Start()
    {
        realMorningPeriod = spawnDelay * spawnCount + morningPeriod;
    }

    private void Update()
    {
        _nowTime += Time.deltaTime;

        if (_nowTime >= realMorningPeriod)
        {
            _spawnPeriod = false;
        }
        else
        {
            _spawnPeriod = true;
        }

        if (Time.time >= _alwaysSpawnTime && _coroutine == null)
        {
            _alwaysCoroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Always));
            _alwaysSpawnTime = Time.time + alwaysPeriod;
        }

        if (!_spawnPeriod && _coroutine == null)
        {
            _coroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Night));
        }
        else if(_spawnPeriod && _coroutine == null)
        {
            _coroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Morning));
        }
    }

    void SpawnRandom(SpawnPrefabType type)
    {
        List<GameObject> spawnPrefabs;
        int y = 0;
        int areaNum = 0;
        Vector3 randomPosition;

        switch (type)
        {
            case SpawnPrefabType.Night:
                spawnPrefabs = enemyPrefabs;
                y = 1;  // 적은 한칸 위에서 생성
                break;
            case SpawnPrefabType.Morning:
                spawnPrefabs = resourcePrefabs;
                break;
            default:
                spawnPrefabs = alwaysPrefabs;
                break;
        }

        if (spawnPrefabs.Count == 0 || spawnAreas.Count == 0)
        {
            Debug.LogWarning("spawnPrefabs 또는 Spawn Areas가 설정되지 않았습니다.");
            return;
        }

        int spawnKind = Random.Range(0, spawnPrefabs.Count);

        GameObject randomPrefab = spawnPrefabs[spawnKind];

        if ((int)type >= spawnAreas.Count - 1)
        {
            Debug.LogWarning($"{type}에 해당하는 스폰 지역이 없습니다.");
            return;
        }

        Rect randomArea = spawnAreas[(int)type];

        do
        {
            randomPosition = new Vector3(
            Random.Range(randomArea.xMin, randomArea.xMax), y,
            Random.Range(randomArea.yMin, randomArea.yMax));
        }
        while (IsPositionOccupiedByOverlapSphere(randomPosition));

        GameObject spawnEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity);

        spawnEnemy.transform.parent = transform;
    }

    bool IsPositionOccupiedByOverlapSphere(Vector3 position)
    {
        int layerMask = LayerMask.GetMask("Enemy", "Resource");  // "Enemy"와 "Resource" 레이어만 확인
        float checkRadius = 1f; // 체크할 반경
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius, layerMask);

        // Collider가 하나라도 있으면 그 위치는 이미 사용 중
        if (hitColliders.Length > 0)
        {
            return true;
        }

        return false;
    }


    IEnumerator DelaySpawn(SpawnPrefabType type)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandom(type);
            yield return new WaitForSeconds(spawnDelay);
        }

        yield return new WaitForSeconds(morningPeriod);

        switch (type)
        {
            case SpawnPrefabType.Morning: _coroutine = null; break;
            case SpawnPrefabType.Night: 
                _coroutine = null;
                _nowTime = 0;
                break;
            case SpawnPrefabType.Always: _alwaysCoroutine = null; break;
        }
    }


    private void OnDrawGizmosSelected()
    {
        gizmoColor = new Color[]
        {
            new Color(0, 1, 0, .3f), new Color(1, 0, 0, .3f), new Color(0, 0, 1, .3f),
            new Color(1, 1, 0, .3f), new Color(1, 0, 1, .3f), new Color(0, 1, 1, .3f)
        };

        int spawnAreasNum = 0;

        if (spawnAreas == null) return;

        foreach (var area in spawnAreas)
        {
            Gizmos.color = gizmoColor[spawnAreasNum % gizmoColor.Length];
            Vector3 center = new Vector3(area.x + area.width / 2, 0, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, 1, area.height);

            Gizmos.DrawCube(center, size);
            spawnAreasNum++;
        }
    }
}