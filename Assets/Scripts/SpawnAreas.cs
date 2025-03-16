using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum SpawnPrefabType
{
    Morning,
    Night,
    Always
}

public class SpawnAreas : MonoBehaviour
{
    public TextMeshProUGUI dayText;

    [SerializeField] List<Rect> spawnAreas;  // 스폰 지역

    [SerializeField] private List<GameObject> enemyPrefabs;  // 적 프리팹

    [SerializeField] private List<GameObject> resourcePrefabs; // 자원 프리팹

    [SerializeField] private List<GameObject> alwaysPrefabs;  // 항상 나오는 프리팹

    [SerializeField] private float spawnDelay;  // 동시 생성 방지, 스폰 딜레이

    [SerializeField] private float alwaysPeriod;  // 항상 나오는 오브젝트 주기

    [SerializeField] private int resourceSpawnCount;  // 자원 생성 갯수

    [SerializeField] private int enemySpawnCount; // 적 생성 갯수

    [SerializeField] private int dDay;  // 지난 날짜

    [SerializeField] private int enemySpawnRateUpDay;  // 적 스폰 비율 증가 날짜

    private float _alwaysSpawnTime = 0f;  // always프리팹 스폰 시간

    private int _countNum = 1; // Enemy 스폰 증가 폭

    Color[] _gizmoColor;

    List<float> _resourceBottomPosition; // 자원 오브젝트 밑 부분
    List<float> _enemyBottomPosition;    // 적 오브젝트 밑 부분

    private Coroutine _coroutine;
    private Coroutine _alwaysCoroutine;

    public SunMoonCycle _sunMoonCycle;

    private void Start()
    { 
        _resourceBottomPosition = new List<float>();
        _enemyBottomPosition = new List<float>();

        GetBottomPositions(resourcePrefabs, _resourceBottomPosition, "Resource");
        GetBottomPositions(enemyPrefabs, _enemyBottomPosition, "Enemy");
    }

    private void Update()
    {
        if (Time.time >= _alwaysSpawnTime && _alwaysCoroutine == null)
        {
            _alwaysCoroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Always));
            _alwaysSpawnTime = Time.time + alwaysPeriod;
        }

        if (_sunMoonCycle.isNight && !_sunMoonCycle.isRote && _coroutine == null)
        {
            _coroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Night));
        }
        else if (!_sunMoonCycle.isNight && _coroutine == null)
        {
            _coroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Morning));
        }
    }

    private void GetBottomPositions(List<GameObject> prefabs, List<float> bottomPositions, string type)
    {
        if (prefabs == null || bottomPositions == null)
        {
            Debug.LogWarning($"{type} Prefabs 또는 리스트가 초기화되지 않았습니다.");
            return;
        }

        foreach (GameObject prefab in prefabs)
        {
            if (prefab.TryGetComponent(out Collider collider))
            {
                bottomPositions.Add(collider.bounds.min.y);
            }
            else
            {
                Debug.LogWarning($"{prefab.name} ({type}) 오브젝트에 Collider가 없습니다!");
            }
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

        if ((int)type > spawnAreas.Count - 1)
        {
            Debug.LogWarning($"{type}에 해당하는 스폰 지역이 없습니다.");
            return;
        }

        Rect randomArea = spawnAreas[(int)type]; // 타입에 맞는 스폰 지역 할당

        do
        {
            randomPosition = new Vector3(
            Random.Range(randomArea.xMin, randomArea.xMax), spawnPositionsY[spawnKind],
            Random.Range(randomArea.yMin, randomArea.yMax));
        }
        while (IsPositionOccupiedByOverlapSphere(randomPosition));  // 타입에 맞는 랜덤 스폰 위치 저장

        GameObject spawnPrefab = Instantiate(randomPrefab, randomPosition, Quaternion.identity);

        spawnPrefab.transform.parent = transform;
    }

    bool IsPositionOccupiedByOverlapSphere(Vector3 position) // 스폰 시 주변 오브젝트 체크 함수
    {
        int layerMask = LayerMask.GetMask("Enemy", "Resource", "Player");  // "Enemy"와 "Resource" 레이어만 확인
        float checkRadius = 1f; // 체크할 반경
        return Physics.CheckSphere(position, checkRadius, layerMask);
    }

    void DayPass()
    {
        if (enemySpawnRateUpDay != 0)
        {
            if (_sunMoonCycle.dDay % enemySpawnRateUpDay == 0)
            {
                enemySpawnCount += _countNum;
                _countNum++;
            }
        }
    }


    IEnumerator DelaySpawn(SpawnPrefabType type)
    {
        int spawnCount = 0;

        if (type == SpawnPrefabType.Morning) spawnCount = resourceSpawnCount;
        else if (type == SpawnPrefabType.Night) spawnCount = enemySpawnCount;

        if (spawnDelay * spawnCount >= 1) spawnDelay = 1 / spawnDelay;

            for (int i = 0; i < spawnCount; i++)
        {
            SpawnRandom(type);
            yield return new WaitForSeconds(spawnDelay);
        }

        if (type == SpawnPrefabType.Night)
        {
            while (_sunMoonCycle.isNight)
            {
                yield return null;
            }
        }
        else if(type == SpawnPrefabType.Morning)
        {
            while (!_sunMoonCycle.isNight)
            {
                yield return null;
            }
        }

        switch (type)
        {
            case SpawnPrefabType.Morning:
                _coroutine = null;
                break;
            case SpawnPrefabType.Night:
                _coroutine = null;
                DayPass();
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