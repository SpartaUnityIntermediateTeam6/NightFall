using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using System;

public class CraftingElement : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI nameTmp;
    [SerializeField] private TextMeshProUGUI descTmp;
    [SerializeField] private Button buildBtn;
    [SerializeField] private GameObject recipeInfoElementPrefab;
    [SerializeField] private GameObject elementParent;

    public void SetElement(ItemRecipeData building, Inventory inventory, Action buttonEvent)
    {
        nameTmp.text = building.resultItem.Name;
        descTmp.text = building.resultItem.Tooltip;
        itemIcon.sprite = building.resultItem.IconSprite;

        buildBtn.onClick.AddListener(() =>
        {
            buttonEvent?.Invoke();
        });

        foreach (var iter in building.recipeDates)
        {
            var go = Instantiate(recipeInfoElementPrefab, elementParent.transform);
            go.SetActive(true);

            //Sample Code
            go.transform.GetChild(0).GetComponent<Image>().sprite = iter.itemData.IconSprite;
            go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text =
                $"{inventory.GetTotalAmount(iter.itemData)} / {iter.requiredAmount}";
        }
    }
}
