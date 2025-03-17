using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject slotContent;
    [SerializeField] private GameObject buildInfoElementPrefab;

    //Sample Code
    private List<GameObject> _slots = new();

    public void UpdateUI(BuildRecipeData data)
    {
        _slots.ForEach(go => Destroy(go));
        _slots.Clear();

        if (data == null)
        {
            content.SetActive(false);
            return;
        }

        content.SetActive(!content.activeInHierarchy);
        
        foreach (var iter in data.recipeDates)
        {
            //Sample Code
            //프리펩 동적생성 or 오브젝트풀
            var go = Instantiate(buildInfoElementPrefab, slotContent.transform);

            go.SetActive(true);
            go.GetComponent<Image>().sprite = iter.itemData.IconSprite;

            //int currentCount = data.Inventory.GetTotalAmount(iter.item);

            //go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
               // $"{currentCount} / {iter.requiredAmount}";

            _slots.Add(go);

            //Sample Code
        }
    }

    void OnEnable() => content.SetActive(false);
}
