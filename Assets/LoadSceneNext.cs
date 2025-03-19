using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneNext : MonoBehaviour
{
    [SerializeField] private SceneAsset sceneAsset;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneAsset.name);
    }
}
