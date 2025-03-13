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

    [SerializeField] List<Rect> spawnAreas;  // ���� ����

    [SerializeField] private List<GameObject> enemyPrefabs;  // �� ������

    [SerializeField] private List<GameObject> resourcePrefabs; // �ڿ� ������

    [SerializeField] private List<GameObject> alwaysPrefabs;  // �׻� ������ ������

    [SerializeField] private float spawnDelay;  // ���� ���� ����, ���� ������

    [SerializeField] private float morningPeriod;  // �㳷 �ֱ�

    [SerializeField] private float alwaysPeriod;  // �׻� ������ ������Ʈ �ֱ�

    [SerializeField] private int spawnCount;  // ���� ����

    private float realMorningPeriod;

    private float _alwaysSpawnTime = 0f;  // always������ ���� �ð�

    private bool _spawnPeriod; // �����ֱ� Ȯ��

    private float _nowTime;

    Color[] _gizmoColor;

    List<float> _resourceBottomPosition = new List<float>(); // �ڿ� ������Ʈ �� �κ�
    List<float> _enemyBottomPosition = new List<float>(); // �� ������Ʈ �� �κ�

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
                // Collider�� �ϴ� �κ��� ��ġ ���
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
            Debug.LogWarning("spawnPrefabs �Ǵ� Spawn Areas�� �������� �ʾҽ��ϴ�.");
            return;
        }

        int spawnKind = Random.Range(0, spawnPrefabs.Count);

        GameObject randomPrefab = spawnPrefabs[spawnKind];

        if ((int)type >= spawnAreas.Count - 1)
        {
            Debug.LogWarning($"{type}�� �ش��ϴ� ���� ������ �����ϴ�.");
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

    bool IsPositionOccupiedByOverlapSphere(Vector3 position) // ���� �� �ֺ� ������Ʈ üũ �Լ�
    {
        int layerMask = LayerMask.GetMask("Enemy", "Resource", "Player");  // "Enemy"�� "Resource" ���̾ Ȯ��
        float checkRadius = 1f; // üũ�� �ݰ�
        Collider[] hitColliders = Physics.OverlapSphere(position, checkRadius, layerMask);

        // Collider�� �ϳ��� ������ �� ��ġ�� �̹� ��� ��
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