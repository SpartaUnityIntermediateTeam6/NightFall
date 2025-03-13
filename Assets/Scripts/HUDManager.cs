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

    [Header("Beacon HP UI")]
    public Image beaconHPBar;
    public TextMeshProUGUI beaconHPText;
    public int maxBeaconHP = 200;
    private int currentBeaconHP;

    void Start()
    {
        // 초기 HP 설정
        currentPlayerHP = maxPlayerHP;
        currentBeaconHP = maxBeaconHP;

        // UI 업데이트
        UpdatePlayerHP();
        UpdateBeaconHP();
    }

    void UpdatePlayerHP()
    {
        float hpRatio = (float)currentPlayerHP / maxPlayerHP;
        playerHPBar.fillAmount = hpRatio;
        playerHPText.text = $"{currentPlayerHP} / {maxPlayerHP}";
    }

    void UpdateBeaconHP()
    {
        float hpRatio = (float)currentBeaconHP / maxBeaconHP;
        beaconHPBar.fillAmount = hpRatio;
        beaconHPText.text = $"{currentBeaconHP} / {maxBeaconHP}";
    }

    // 플레이어가 피해를 받을 때 호출
    public void TakeDamagePlayer(int damage)
    {
        currentPlayerHP = Mathf.Max(0, currentPlayerHP - damage);
        UpdatePlayerHP();
    }

    // 방어 목표 (Beacon)이 피해를 받을 때 호출
    public void TakeDamageBeacon(int damage)
    {
        currentBeaconHP = Mathf.Max(0, currentBeaconHP - damage);
        UpdateBeaconHP();
    }
}
