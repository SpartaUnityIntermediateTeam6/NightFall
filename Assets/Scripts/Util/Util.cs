using System;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : MonoBehaviour
    {
        return go.TryGetComponent<T>(out T component) ?  component : go.AddComponent<T>();
    }

    /// <summary>
    /// Resources/Prefab ���� ���� ������ ��ο��� Prefab�� �ε��Ͽ� ���̾��Ű�� �߰��մϴ�.
    /// </summary>
    public static GameObject InstantiatePrefab(string path, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
    {
        GameObject go = Resources.Load<GameObject>($"Prefabs/{path}");
        if (go == null)
        {
            throw new InvalidOperationException($"Failed to Load Prefab: {path}");
        }

        if(parent != null) return GameObject.Instantiate(go, parent, false);
        else return GameObject.Instantiate(go, position, rotation, parent);
    }

    /// <summary>
    /// Resources/Prefab ���� ���� ������ ��ο��� Prefab�� �ε��Ͽ� �߰��� ���̾��Ű�� ������Ʈ�� �����ɴϴ�.
    /// </summary>
    public static T InstantiatePrefabAndGetComponent<T>(string path, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component
    {
        T comp = InstantiatePrefab(path, position, rotation, parent).GetComponent<T>();
        if (comp == null)
        {
            throw new InvalidOperationException($"Prefab instantiated but component of type {typeof(T)} not found in {path}");
        }
        return comp;
    }
}
