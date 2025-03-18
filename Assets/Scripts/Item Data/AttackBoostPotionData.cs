using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBoostPotion", menuName = "Items/AttackBoostPotion")]
public class AttackBoostPotionData : CountableItemData
{
    [SerializeField] private float attackBoostAmount = 5f; // 증가할 공격력 값

    public float AttackBoostAmount => attackBoostAmount;

    // ✅ 아이템 객체 생성 메서드 구현 (필수)
    public override Item CreateItem()
    {
        return new AttackBoostPotion(this, 1);
    }
}



