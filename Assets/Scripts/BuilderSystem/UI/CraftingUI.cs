using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private GameObject elementParent;
    [SerializeField] private GameObject craftingInfoElementPrefab;

    //Sample Code
    private readonly List<GameObject> _elementCache = new();

    void OnEnable()
    {
        EventBus.Subscribe<CraftingInteractionEvent>(OnBuildInteraction);
        view.SetActive(false);
    }

    void OnDisable() => EventBus.Unsubscribe<CraftingInteractionEvent>(OnBuildInteraction);

    private void OnBuildInteraction(CraftingInteractionEvent eventInfo)
    {
        if (eventInfo == null)
            return;

        _elementCache.ForEach(s => Destroy(s));
        _elementCache.Clear();
        view.gameObject.SetActive(true);

        for (int i = 0; i < eventInfo.recipes.Count; i++)
        {
            var index = i;
            var go = Instantiate(craftingInfoElementPrefab, elementParent.transform).GetComponent<CraftingElement>();

            go.gameObject.SetActive(true);
            go.SetElement(eventInfo.recipes[i], eventInfo.inventory, () =>
            {
                eventInfo.onButtonEvent.Invoke(index);
            });

            _elementCache.Add(go.gameObject);
        }
    }

    public void Show(bool on)
    {
        view.gameObject.SetActive(on);
    }
}
