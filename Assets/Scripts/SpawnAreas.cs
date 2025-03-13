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
                y = 1;  // ���� ��ĭ ������ ����
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
            Random.Range(randomArea.xMin, randomArea.xMax), y,
            Random.Range(randomArea.yMin, randomArea.yMax));
        }
        while (IsPositionOccupiedByOverlapSphere(randomPosition));

        GameObject spawnEnemy = Instantiate(randomPrefab, randomPosition, Quaternion.identity);

        spawnEnemy.transform.parent = transform;
    }

    bool IsPositionOccupiedByOverlapSphere(Vector3 position)
    {
        int layerMask = LayerMask.GetMask("Enemy", "Resource");  // "Enemy"�� "Resource" ���̾ Ȯ��
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