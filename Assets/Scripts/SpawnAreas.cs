using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum SpawnPrefabType
{
    Morning,
    Night,
    Always
}

public class SpawnAreas : Poolable
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

    [SerializeField] private int maxResourceCount;

    private List<GameObject> _activeMorningObjects = new List<GameObject>();

    private List<Poolable> _enemyList = new List<Poolable>();

    private float _alwaysSpawnTime = 0f;  // always프리팹 스폰 시간

    private int _countNum = 1; // Enemy 스폰 증가 폭

    Color[] _gizmoColor;


    private Coroutine _coroutine;
    private Coroutine _alwaysCoroutine;

    public SunMoonCycle _sunMoonCycle;


    private void Update()
    {
        if (Time.time >= _alwaysSpawnTime && _alwaysCoroutine == null)
        {
            _alwaysCoroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Always));
            _alwaysSpawnTime = Time.time + alwaysPeriod;
        }

        if (_sunMoonCycle.dDay <= 3)
        {
            if (_sunMoonCycle.isNight && !_sunMoonCycle.isRote && _coroutine == null)
            {
                _coroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Night));
            }
            else if (!_sunMoonCycle.isNight && _coroutine == null)
            {
                _coroutine = StartCoroutine(DelaySpawn(SpawnPrefabType.Morning));

                foreach (Poolable obj in _enemyList)
                {
                    GameManager.Instance.poolManager.Release(obj);
                }
                _enemyList.Clear();
            }
        }
        else
        {
            if (_coroutine == null)
            {
                enemySpawnCount = 300;
                _coroutine = StartCoroutine(EndingGame());
            }
        }
    }


    void SpawnRandom(SpawnPrefabType type)
    {
        if (type == SpawnPrefabType.Morning && _activeMorningObjects.Count >= maxResourceCount)
        {
            //Debug.Log("이미 최대 개수입니다.");
            return;
        }

        List<GameObject> spawnPrefabs;

        Vector3 randomPosition;

        switch (type)
        {
            case SpawnPrefabType.Night:
                spawnPrefabs = enemyPrefabs;
                break;
            case SpawnPrefabType.Morning:
                spawnPrefabs = resourcePrefabs;
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
            Random.Range(randomArea.xMin, randomArea.xMax), 100f,
            Random.Range(randomArea.yMin, randomArea.yMax));

            RaycastHit hit;
            if (Physics.Raycast(randomPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                randomPosition.y = hit.point.y; // 충돌 지점의 Y 좌표 사용
            }
            else
            {
                Debug.LogWarning("Raycast 실패: 바닥을 찾지 못함");
                return;
            }
        }
        while (IsPositionOccupiedByOverlapSphere(randomPosition));  // 타입에 맞는 랜덤 스폰 위치 저장

        Poolable poolable;

        
        poolable = GameManager.Instance.poolManager.Get(randomPrefab);

        poolable.transform.position = randomPosition;
        poolable.transform.rotation = Quaternion.identity;

        if(type == SpawnPrefabType.Night) _enemyList.Add(poolable);

        if (type == SpawnPrefabType.Morning)
        {
            _activeMorningObjects.Add(poolable.gameObject);
            poolable.gameObject.AddComponent<DestroyCallback>().OnDestroyed += () => _activeMorningObjects.Remove(poolable.gameObject);
        }
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

        if (spawnDelay * spawnCount >= 1) spawnDelay = 0.5f / (float)spawnCount;

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
        else if (type == SpawnPrefabType.Morning)
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

    IEnumerator EndingGame()
    {
        for (int i = 0; i < enemySpawnCount; i++)
        {
            SpawnRandom(SpawnPrefabType.Night);
            yield return new WaitForSeconds(0.05f);
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