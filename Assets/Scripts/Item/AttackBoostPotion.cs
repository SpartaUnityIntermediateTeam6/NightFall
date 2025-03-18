using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoostPotion : CountableItem, IUsableItem
{
    // ✅ `SetData()`를 사용하여 데이터 설정
    public void Initialize(AttackBoostPotionData data)
    {
        this.SetData(data);
        this.SetAmount(1); // 기본 수량을 1로 설정
    }

    public bool Use()
    {
        PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (playerStats == null) return false;

        AttackBoostPotionData potionData = Data as AttackBoostPotionData;
        if (potionData == null) return false;

        // 공격력 증가 적용
        playerStats.UpgradeAttackPower(potionData.AttackBoostAmount);
        Debug.Log($"🧪 {potionData.AttackBoostAmount} 만큼 공격력이 증가했습니다!");

        return true;
    }

    protected override CountableItem Clone(int amount)
    {
        AttackBoostPotion clone = new AttackBoostPotion();
        clone.Initialize(this.Data as AttackBoostPotionData);
        clone.SetAmount(amount);
        return clone;
    }
}



