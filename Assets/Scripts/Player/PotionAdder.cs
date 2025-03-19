using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionAdder : MonoBehaviour
{
    private Inventory inventory;

    [SerializeField] private AttackBoostPotionData attackBoostPotionData; // ✅ 인스펙터에서 직접 참조

    void Start()
    {
        inventory = FindObjectOfType<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("❌ Inventory를 찾을 수 없습니다. PotionAdder를 사용하려면 Inventory가 필요합니다.");
            return;
        }
    }

    public void AddPotionToInventory()
    {
        if (attackBoostPotionData != null)
        {
            inventory.Add(attackBoostPotionData, 1);
            Debug.Log("🎁 공격력 강화 포션이 인벤토리에 추가되었습니다!");
        }
        else
        {
            Debug.LogWarning("❌ AttackBoostPotionData가 인스펙터에서 설정되지 않았습니다!");
        }
    }
}



