using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Player HP UI")]
    public Image playerHPBar;
    public TextMeshProUGUI playerHPText;

    [Header("Sanity UI")]
    public Image sanityBar;
    public TextMeshProUGUI sanityText;

    [Header("Beacon HP UI")]
    public Image beaconHPBar;
    public TextMeshProUGUI beaconHPText;
    public int maxBeaconHP = 200;
    private int _currentBeaconHP;

    private PlayerStats playerStats;

    void Start()
    {
        // PlayerStats 찾기
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            Debug.Log("PlayerStats 연결됨.");
            playerStats.OnHPChanged += UpdatePlayerHP;
            playerStats.OnSanityChanged += UpdateSanity;

            // 초기 UI 업데이트 (PlayerStats에서 이벤트가 정상 실행되지 않을 경우 대비)
            UpdatePlayerHP(playerStats.maxHP, playerStats.maxHP);
            UpdateSanity(playerStats.maxSanity, playerStats.maxSanity);
        }
        else
        {
            Debug.LogError("PlayerStats를 찾을 수 없습니다!");
        }

        // Beacon HP 초기화
        _currentBeaconHP = maxBeaconHP;
        UpdateBeaconHP();
    }

    void UpdatePlayerHP(int currentHP, int maxHP)
    {
        if (playerHPBar != null)
        {
            playerHPBar.fillAmount = (float)currentHP / maxHP;
        }

        if (playerHPText != null)
        {
            playerHPText.text = $"{currentHP} / {maxHP}";
        }
    }

    void UpdateSanity(int currentSanity, int maxSanity)
    {
        if (sanityBar != null)
        {
            sanityBar.fillAmount = (float)currentSanity / maxSanity;
        }

        if (sanityText != null)
        {
            sanityText.text = $"{currentSanity} / {maxSanity}";
        }
    }

    void UpdateBeaconHP()
    {
        if (beaconHPBar != null)
        {
            float hpRatio = (float)_currentBeaconHP / maxBeaconHP;
            beaconHPBar.fillAmount = hpRatio;
        }

        if (beaconHPText != null)
        {
            beaconHPText.text = $"{_currentBeaconHP} / {maxBeaconHP}";
        }
    }

    public void TakeDamageBeacon(int damage)
    {
        _currentBeaconHP = Mathf.Max(0, _currentBeaconHP - damage);
        UpdateBeaconHP();
    }
}



