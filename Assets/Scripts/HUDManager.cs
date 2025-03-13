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
    private int currentBeaconHP;

    private PlayerStats playerStats;

    void Start()
    {
        // PlayerStats ã��
        playerStats = FindObjectOfType<PlayerStats>();

        if (playerStats != null)
        {
            Debug.Log("PlayerStats �����.");
            playerStats.OnHPChanged += UpdatePlayerHP;
            playerStats.OnSanityChanged += UpdateSanity;

            // �ʱ� UI ������Ʈ (PlayerStats���� �̺�Ʈ�� ���� ������� ���� ��� ���)
            UpdatePlayerHP(playerStats.maxHP, playerStats.maxHP);
            UpdateSanity(playerStats.maxSanity, playerStats.maxSanity);
        }
        else
        {
            Debug.LogError("PlayerStats�� ã�� �� �����ϴ�!");
        }

        // Beacon HP �ʱ�ȭ
        currentBeaconHP = maxBeaconHP;
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
            float hpRatio = (float)currentBeaconHP / maxBeaconHP;
            beaconHPBar.fillAmount = hpRatio;
        }

        if (beaconHPText != null)
        {
            beaconHPText.text = $"{currentBeaconHP} / {maxBeaconHP}";
        }
    }

    public void TakeDamageBeacon(int damage)
    {
        currentBeaconHP = Mathf.Max(0, currentBeaconHP - damage);
        UpdateBeaconHP();
    }
}



