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

    //Sample Code
    private List<GameObject> _slots = new();

    public void UpdateUI(RecipeDataSender data)
    {
        _slots.ForEach(go => Destroy(go));
        _slots.Clear();

        content.SetActive(!content.activeInHierarchy);
        
        foreach (var iter in data.recipeData.dates)
        {
            //Sample Code
            //프리펩 동적생성 or 오브젝트풀
            var go = Instantiate(recipePrefab, slotContent.transform);

            go.SetActive(true);
            go.GetComponent<Image>().sprite = iter.item.IconSprite;
            go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                iter.requiredAmount.ToString();

            _slots.Add(go);

            //Sample Code
        }

        buildBtn.onClick.RemoveAllListeners();
        buildBtn.onClick.AddListener(data.OnClickEvent.Invoke);
    }

    void OnEnable() => content.SetActive(false);
}
