using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] List<Rect> spawnAreas;  // ���� ����

    [SerializeField] private List<GameObject> enemyPrefabs;  // �� ������

    [SerializeField] private List<GameObject> resourcePrefabs; // �ڿ� ������

    [SerializeField] private List<GameObject> alwaysPrefabs;  // �׻� ������ ������

    [SerializeField] private float spawnDelay;  // ���� ���� ����, ���� ������

    [SerializeField] private float morningPeriod;  // �㳷 �ֱ�

    [SerializeField] private float alwaysPeriod;  // �׻� ������ ������Ʈ �ֱ�

    [SerializeField] private int resourceSpawnCount;  // �ڿ� ���� ����

    [SerializeField] private int enemySpawnCount; // �� ���� ����

    [SerializeField] private int dDay;  // ���� ��¥

    [SerializeField] private int enemySpawnRateUpDay;  // �� ���� ���� ���� ��¥

    private float realMorningPeriod;

    private float _alwaysSpawnTime = 0f;  // always������ ���� �ð�

    private bool _spawnPeriod; // �����ֱ� Ȯ��

    private float _nowTime;

    private int _countNum = 1;

    Color[] _gizmoColor;

    List<float> _resourceBottomPosition; // �ڿ� ������Ʈ �� �κ�
    List<float> _enemyBottomPosition;    // �� ������Ʈ �� �κ�

    private Coroutine _coroutine;
    private Coroutine _alwaysCoroutine;

    private void Start()
    { 
        _resourceBottomPosition = new List<float>();
        _enemyBottomPosition = new List<float>();

        GetBottomPositions(resourcePrefabs, _resourceBottomPosition, "Resource");
        GetBottomPositions(enemyPrefabs, _enemyBottomPosition, "Enemy");
    }

    private void Update()
    {
        realMorningPeriod = (resourceSpawnCount > enemySpawnCount) ? (spawnDelay * resourceSpawnCount + morningPeriod) : (spawnDelay * enemySpawnCount + morningPeriod);

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

    private void GetBottomPositions(List<GameObject> prefabs, List<float> bottomPositions, string type)
    {
        if (prefabs == null || bottomPositions == null)
        {
            Debug.LogWarning($"{type} Prefabs �Ǵ� ����Ʈ�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
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
                Debug.LogWarning($"{prefab.name} ({type}) ������Ʈ�� Collider�� �����ϴ�!");
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
            Debug.LogWarning("spawnPrefabs �Ǵ� Spawn Areas�� �������� �ʾҽ��ϴ�.");
            return;
        }

        int spawnKind = Random.Range(0, spawnPrefabs.Count);

        GameObject randomPrefab = spawnPrefabs[spawnKind];

        if ((int)type > spawnAreas.Count - 1)
        {
            Debug.LogWarning($"{type}�� �ش��ϴ� ���� ������ �����ϴ�.");
            return;
        }

        Rect randomArea = spawnAreas[(int)type]; // Ÿ�Կ� �´� ���� ���� �Ҵ�

        do
        {
            randomPosition = new Vector3(
            Random.Range(randomArea.xMin, randomArea.xMax), spawnPositionsY[spawnKind],
            Random.Range(randomArea.yMin, randomArea.yMax));
        }
        while (IsPositionOccupiedByOverlapSphere(randomPosition));  // Ÿ�Կ� �´� ���� ���� ��ġ ����

        GameObject spawnPrefab = Instantiate(randomPrefab, randomPosition, Quaternion.identity);

        spawnPrefab.transform.parent = transform;
    }

    bool IsPositionOccupiedByOverlapSphere(Vector3 position) // ���� �� �ֺ� ������Ʈ üũ �Լ�
    {
        int layerMask = LayerMask.GetMask("Enemy", "Resource", "Player");  // "Enemy"�� "Resource" ���̾ Ȯ��
        float checkRadius = 1f; // üũ�� �ݰ�
        return Physics.CheckSphere(position, checkRadius, layerMask);
    }

    void DayPass()
    {
        dDay++;
        if (dDay % enemySpawnRateUpDay == 0)
        {
            enemySpawnCount += _countNum;
            _countNum++;
        }
    }


    IEnumerator DelaySpawn(SpawnPrefabType type)
    {
        int spawnCount = 0;

        if (type == SpawnPrefabType.Morning) spawnCount = resourceSpawnCount;
        else if (type == SpawnPrefabType.Night) spawnCount = enemySpawnCount;

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