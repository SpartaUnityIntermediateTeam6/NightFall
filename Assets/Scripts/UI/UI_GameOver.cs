using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    public GameObject gameoverPanel;

    public Button retryButton;
    public Button exitButton;

    public string startSceneName = "KSM_Start";

    private void Awake()
    {
        retryButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        exitButton.onClick.AddListener(() => SceneManager.LoadScene(startSceneName));
    }

    private void Start()
    {
        gameoverPanel.SetActive(false);
    }

    public void SetGameOverUI(bool flag)
    {
        Debug.Log("게임오버 UI 켜짐");
        gameoverPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
