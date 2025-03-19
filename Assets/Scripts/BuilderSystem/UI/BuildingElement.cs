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

    private List<Action> _actionCache = new();

    public void SetElement(Building building, Inventory inventory, Action buttonEvent)
    {
        nameTmp.text = building.BuildingName;
        descTmp.text = building.BuildingDescription;

        buildBtn.onClick.RemoveAllListeners();
        buildBtn.onClick.AddListener(() =>
        {
            buttonEvent?.Invoke();
        });

        foreach (var iter in building.RecipeData.recipeDates)
        {
            var go = Instantiate(recipeInfoElementPrefab, elementParent.transform).GetComponent<ItemElement>();
            go.gameObject.SetActive(true);
            go.SetIcon(iter.itemData.IconSprite);
            go.SetText($"{inventory.GetTotalAmount(iter.itemData)} / {iter.requiredAmount}");

            _actionCache.Add(() =>
            {
                go.SetText($"{inventory.GetTotalAmount(iter.itemData)} / {iter.requiredAmount}");
            });
        }
    }

    public void OnChangedInventory(Inventory inventory)
    {
        foreach (var iter in _actionCache)
        {
            iter?.Invoke();
        }
    }
}
