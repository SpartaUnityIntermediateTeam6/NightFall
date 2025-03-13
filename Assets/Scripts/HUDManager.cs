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
    public int maxPlayerHP = 100;
    private int currentPlayerHP;

    [Header("Sanity UI")]
    public Image sanityBar;
    public TextMeshProUGUI sanityText;
    public int maxSanity = 100;
    private int currentSanity;

    [Header("Beacon HP UI")]
    public Image beaconHPBar;
    public TextMeshProUGUI beaconHPText;
    public int maxBeaconHP = 200;
    private int currentBeaconHP;

    void Start()
    {
        // UI 자동 할당 (수동 연결이 안 되어 있을 경우 대비)
        if (playerHPBar == null)
            playerHPBar = GameObject.Find("PlayerHPBar").GetComponent<Image>();

        if (playerHPText == null)
            playerHPText = GameObject.Find("PlayerHPText").GetComponent<TextMeshProUGUI>();

        if (sanityBar == null)
            sanityBar = GameObject.Find("SanityBar").GetComponent<Image>();

        if (sanityText == null)
            sanityText = GameObject.Find("SanityText").GetComponent<TextMeshProUGUI>();

        if (beaconHPBar == null)
            beaconHPBar = GameObject.Find("BeaconHPBar").GetComponent<Image>();

        if (beaconHPText == null)
            beaconHPText = GameObject.Find("BeaconHPText").GetComponent<TextMeshProUGUI>();

        // 초기 상태 설정
        currentPlayerHP = maxPlayerHP;
        currentSanity = maxSanity;
        currentBeaconHP = maxBeaconHP;

        // UI 업데이트
        UpdatePlayerHP();
        UpdateSanity();
        UpdateBeaconHP();
    }

    void UpdatePlayerHP()
    {
        if (playerHPBar != null)
        {
            float hpRatio = (float)currentPlayerHP / maxPlayerHP;
            playerHPBar.fillAmount = hpRatio;
        }

        if (playerHPText != null)
        {
            playerHPText.text = $"{currentPlayerHP} / {maxPlayerHP}";
        }
    }

    void UpdateSanity()
    {
        if (sanityBar != null)
        {
            float sanityRatio = (float)currentSanity / maxSanity;
            sanityBar.fillAmount = sanityRatio;
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
            float hpRatio = (float)currentBeaconHP / maxBeaconHP;
            beaconHPBar.fillAmount = hpRatio;
        }

        if (beaconHPText != null)
        {
            beaconHPText.text = $"{currentBeaconHP} / {maxBeaconHP}";
        }
    }

    // 플레이어 체력 감소
    public void TakeDamagePlayer(int damage)
    {
        currentPlayerHP = Mathf.Max(0, currentPlayerHP - damage);
        UpdatePlayerHP();
    }

    // 정신력 감소
    public void ReduceSanity(int amount)
    {
        currentSanity = Mathf.Max(0, currentSanity - amount);
        UpdateSanity();
    }

    // 방어 목표 (Beacon) 체력 감소
    public void TakeDamageBeacon(int damage)
    {
        currentBeaconHP = Mathf.Max(0, currentBeaconHP - damage);
        UpdateBeaconHP();
    }
}

