using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackBoostPotion", menuName = "Items/AttackBoostPotion")]
public class AttackBoostPotionData : CountableItemData
{
    [SerializeField] private float attackBoostAmount = 5f; // 증가할 공격력 값

    public float AttackBoostAmount => attackBoostAmount;

    public override Item CreateItem()
    {
        AttackBoostPotion potion = new AttackBoostPotion();
        potion.Initialize(this); //  올바르게 Initialize() 호출
        return potion;
    }
}


