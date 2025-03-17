using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTmp;
    [SerializeField] private TextMeshProUGUI descTmp;
    [SerializeField] private Button buildBtn;
    [SerializeField] private GameObject recipeInfoElementPrefab;
    [SerializeField] private GameObject elementParent;

    public void SetBuildingInfo(Building building, Inventory inventory, Action buttonEvent)
    {
        nameTmp.text = building.BuildingName;
        descTmp.text = building.BuildingDescription;
        buildBtn.onClick.AddListener(() =>
        {
            buttonEvent?.Invoke();
        });

        foreach (var iter in building.RecipeData.recipeDates)
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
