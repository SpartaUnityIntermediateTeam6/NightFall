using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Start : MonoBehaviour
{
    public Button startButton;
    public Button optionButton;
    public Button exitButton;

    public string mainSceneName = "KSM";

    public GameObject mainPanel;
    public GameObject optionPanel;
    public Button optionCloseButton;

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Awake()
    {
        startButton.onClick.AddListener(() => SceneManager.LoadScene(mainSceneName));
        optionButton.onClick.AddListener(OpenOption);
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

        optionCloseButton.onClick.AddListener(CloseOption);

        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        CloseOption();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBGM("BGM_Day");

        masterSlider.value = SoundManager.Instance.GetMasterVolume();
        bgmSlider.value = SoundManager.Instance.GetBGMVolume();
        sfxSlider.value = SoundManager.Instance.GetSFXVolume();
    }

    void OpenOption()
    {
        optionPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    void CloseOption()
    {
        optionPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    void OnMasterSliderChanged(float value)
    {
        masterSlider.value = value;
        SoundManager.Instance.SetMasterVolume(masterSlider.value);
    }
    void OnBGMSliderChanged(float value)
    {
        bgmSlider.value = value;
        SoundManager.Instance.SetBGMVolume(bgmSlider.value);
    }
    void OnSFXSliderChanged(float value)
    {
        sfxSlider.value = value;
        SoundManager.Instance.SetSFXVolume(sfxSlider.value);
    }
}
