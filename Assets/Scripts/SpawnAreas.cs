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

    Color[] _gizmoColor;

    List<float> _resourceBottomPosition = new List<float>(); // 자원 오브젝트 밑 부분
    List<float> _enemyBottomPosition = new List<float>(); // 적 오브젝트 밑 부분

    private Coroutine _coroutine;
    private Coroutine _alwaysCoroutine;

    private void Start()
    {
        realMorningPeriod = spawnDelay * spawnCount + morningPeriod;

        for (int i = 0; i < resourcePrefabs.Count; i++)
        {
            Collider collider = resourcePrefabs[i].GetComponent<Collider>();

            if (collider != null)
            {
                // Collider의 하단 부분의 위치 계산
                Vector3 bottomPosition = collider.bounds.min;
                _resourceBottomPosition.Add(bottomPosition.y);
            }
            else
            {
                Debug.LogWarning($"{resourcePrefabs[i].name} does not have a Collider!");
            }
        }

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            Collider collider = enemyPrefabs[i].GetComponent<Collider>();

            if (collider != null)
            {
                Vector3 bottomPosition = collider.bounds.min;
                _enemyBottomPosition.Add(bottomPosition.y);
            }
            else
            {
                Debug.LogWarning($"{enemyPrefabs[i].name} does not have a Collider!");
            }
        }
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

        if (Time.time >= _alwaysSpawnTime && _alwaysCoroutine == null)
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
        List<float> spawnPositionsY = new List<float>();

        Vector3 randomPosition;

        switch (type)
        {
            case SpawnPrefabType.Night:
                spawnPrefabs = enemyPrefabs;
                spawnPositionsY = _enemyBottomPosition;
                break;
            case SpawnPrefabType.Morning:
                spawnPrefabs = resourcePrefabs;
                spawnPositionsY = _resourceBottomPosition;
                break;
            default:
                spawnPrefabs = alwaysPrefabs;
                break;
        }

        if (type == SpawnPrefabType.Always) return;

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
            Random.Range(randomArea.xMin, randomArea.xMax), spawnPositionsY[spawnKind],
            Random.Range(randomArea.yMin, randomArea.yMax));
        }
        while (IsPositionOccupiedByOverlapSphere(randomPosition));

        GameObject spawnPrefab = Instantiate(randomPrefab, randomPosition, Quaternion.identity);

        spawnPrefab.transform.parent = transform;
    }

    bool IsPositionOccupiedByOverlapSphere(Vector3 position) // 스폰 시 주변 오브젝트 체크 함수
    {
        int layerMask = LayerMask.GetMask("Enemy", "Resource", "Player");  // "Enemy"와 "Resource" 레이어만 확인
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
        _gizmoColor = new Color[]
        {
            new Color(0, 1, 0, .3f), new Color(1, 0, 0, .3f), new Color(0, 0, 1, .3f),
            new Color(1, 1, 0, .3f), new Color(1, 0, 1, .3f), new Color(0, 1, 1, .3f)
        };

        int spawnAreasNum = 0;

        if (spawnAreas == null) return;

        foreach (var area in spawnAreas)
        {
            Gizmos.color = _gizmoColor[spawnAreasNum % _gizmoColor.Length];
            Vector3 center = new Vector3(area.x + area.width / 2, 0, area.y + area.height / 2);
            Vector3 size = new Vector3(area.width, 1, area.height);

            Gizmos.DrawCube(center, size);
            spawnAreasNum++;
        }
    }
}