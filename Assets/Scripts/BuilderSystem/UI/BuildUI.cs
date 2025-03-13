using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject slotContent;
    [SerializeField] private GameObject recipePrefab;

    public void UpdateUI(RecipeData data)
    {
        Debug.Log(data.dates.Count);
        
        foreach (var iter in data.dates)
        {
            //Sample Code
            var go = Instantiate(recipePrefab, slotContent.transform);

            go.SetActive(true);
            go.GetComponent<Image>().sprite = iter.item.IconSprite;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                iter.requiredAmount.ToString();
        }
    }

    //void OnEnable() => content.SetActive(false);
}
