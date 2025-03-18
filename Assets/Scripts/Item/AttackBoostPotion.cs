using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoostPotion : CountableItem, IUsableItem
{
    // 생성자 (아이템 데이터와 개수 설정)
    public AttackBoostPotion(AttackBoostPotionData data, int amount = 1) : base(data, amount) { }

    // ✅ 아이템 사용 기능 구현
    public bool Use()
    {
        PlayerStats playerStats = GameObject.FindObjectOfType<PlayerStats>();
        if (playerStats == null) return false;

        AttackBoostPotionData potionData = Data as AttackBoostPotionData;
        if (potionData == null) return false;

        // 공격력 증가 적용
        playerStats.UpgradeAttackPower(potionData.AttackBoostAmount);
        Debug.Log($"🧪 {potionData.AttackBoostAmount} 만큼 공격력이 증가했습니다!");

        // 수량 감소
        Amount--;
        return true;
    }

    // ✅ 수량을 지정한 복제본을 만드는 메서드 (필수 구현)
    protected override CountableItem Clone(int amount)
    {
        return new AttackBoostPotion(Data as AttackBoostPotionData, amount);
    }
}



