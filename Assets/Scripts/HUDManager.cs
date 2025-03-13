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
        // �ʱ� HP ����
        currentPlayerHP = maxPlayerHP;
        currentBeaconHP = maxBeaconHP;

        // UI ������Ʈ
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

    // �÷��̾ ���ظ� ���� �� ȣ��
    public void TakeDamagePlayer(int damage)
    {
        currentPlayerHP = Mathf.Max(0, currentPlayerHP - damage);
        UpdatePlayerHP();
    }

    // ��� ��ǥ (Beacon)�� ���ظ� ���� �� ȣ��
    public void TakeDamageBeacon(int damage)
    {
        currentBeaconHP = Mathf.Max(0, currentBeaconHP - damage);
        UpdateBeaconHP();
    }
}
