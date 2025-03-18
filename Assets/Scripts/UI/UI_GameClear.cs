using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameClear : MonoBehaviour
{
    public GameObject gameClearPanel;

    public Button exitButton;

    public string startSceneName = "KSM_Start";
    private void Awake()
    {
        exitButton.onClick.AddListener(() => SceneManager.LoadScene(startSceneName));
    }

    private void Start()
    {
        gameClearPanel.SetActive(false);
    }

    public void SetGameClearUI()
    {
        gameClearPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
