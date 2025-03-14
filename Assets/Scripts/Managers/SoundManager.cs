using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<AudioClip> BGM;//배경음악 모음
    [SerializeField] private List<AudioClip> SFX;//효과음 모음

    [SerializeField] private AudioSource bgmPlayer = new AudioSource();//배경음악 재생기
    [SerializeField] private List<AudioSource> sfxPlayer = new List<AudioSource>();//효과음 재생기
    public int maxPoolSize = 10;

    public AudioMixer audioMixer;
    readonly string MIXER_MASTER = "MASTER";//모든 볼륨
    readonly string MIXER_BGM = "BGM";//배경 볼륨
    readonly string MIXER_SFX = "SFX";//효과음 볼륨

    protected override void Awake()
    {
        base.Awake();

        BGM = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sounds/BGM"));
        SFX = new List<AudioClip>(Resources.LoadAll<AudioClip>("Sounds/SFX"));

        bgmPlayer = transform.GetChild(0).GetComponent<AudioSource>();
        sfxPlayer = new List<AudioSource>(transform.GetChild(1).GetComponentsInChildren<AudioSource>());
    }

    private void Start()
    {
        LoadVolume();
    }

    public void PlayBGM(string name)//배경음악 재생
    {
        foreach (AudioClip clip in BGM)
        {
            if(clip.name == name)
            {
                bgmPlayer.clip = clip;
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()//배경음악 종료
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string name)//효과음 재생
    {
        for(int i = 0; i < SFX.Count; i++)
        {
            if (SFX[i].name == name)
            {
                for (int j = 0; j < sfxPlayer.Count; j++)
                {
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = SFX[i];
                        sfxPlayer[j].Play();
                        return;
                    }
                }

                GameObject sfx = new GameObject(name);
                sfx.transform.SetParent(transform);
                AudioSource newSource = sfx.AddComponent<AudioSource>();
                newSource.clip = SFX[i];
                newSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups(MIXER_SFX)[0];
                newSource.loop = false;
                newSource.Play();
                sfxPlayer.Add(newSource);
                StartCoroutine(DestoryAudiosource(newSource));
                return;
            }
        }
    }

    public void StopAllSFX()//모든 효과음 종료
    {
        foreach(AudioSource s in sfxPlayer)
        {
            s.Stop();
        }
    }

    IEnumerator DestoryAudiosource(AudioSource source)//효과음 재생 끝나면 오디오소스 삭제
    {
        yield return new WaitWhile(() => source.isPlaying);

        sfxPlayer.Remove(source);
        Destroy(source.gameObject);
    }

    public void StopAllSound()//모든 음악 종료
    {
        StopBGM();
        StopAllSFX();
    }

    public float GetMasterVolume()
    {
        audioMixer.GetFloat(MIXER_MASTER, out float volume);
        return Mathf.Pow(10, volume / 20);
    }

    public void SetMasterVolume(float volume)//전체 볼륨 조절(0~1)
    {
        volume = Mathf.Max(volume, 0.0001f);
        audioMixer.SetFloat(MIXER_MASTER, Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("MasterVolume", GetMasterVolume());
    }

    public float GetBGMVolume()
    {
        audioMixer.GetFloat(MIXER_BGM, out float volume);
        return Mathf.Pow(10, volume / 20);
    }

    public void SetBGMVolume(float volume)//배경음악 볼륨 조절(0~1)
    {
        volume = Mathf.Max(volume, 0.0001f);
        audioMixer.SetFloat(MIXER_BGM, Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("BGMVolume", GetBGMVolume());
    }

    public float GetSFXVolume()
    {
        audioMixer.GetFloat(MIXER_SFX, out float volume);
        return Mathf.Pow(10, volume / 20);
    }

    public void SetSFXVolume(float volume)//효과음 볼륨 조절(0~1)
    {
        volume = Mathf.Max(volume, 0.0001f);
        audioMixer.SetFloat(MIXER_SFX, Mathf.Log10(volume) * 20);

        PlayerPrefs.SetFloat("SFXVolume", GetSFXVolume());
    }

    public void LoadVolume()
    {
        SetMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 1f));
        SetBGMVolume(PlayerPrefs.GetFloat("BGMVolume", 1f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 1f));
    }
}
