using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private Button buildBtn;
    [SerializeField] private GameObject slotContent;
    [SerializeField] private GameObject recipePrefab;

    public void UpdateUI(RecipeQuery data)
    {
        content.SetActive(!content.activeInHierarchy);
        
        foreach (var iter in data.recipeData.dates)
        {
            //Sample Code
            var go = Instantiate(recipePrefab, slotContent.transform);

            go.SetActive(true);
            go.GetComponent<Image>().sprite = iter.item.IconSprite;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                iter.requiredAmount.ToString();

            //Sample Code
        }

        buildBtn.onClick.RemoveAllListeners();
        buildBtn.onClick.AddListener(data.queryEvent.Invoke);
    }

    void OnEnable() => content.SetActive(false);
}
