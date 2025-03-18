using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUI : MonoBehaviour
{
    [SerializeField] private GameObject view;
    [SerializeField] private GameObject elementParent;
    [SerializeField] private GameObject buildInfoElementPrefab;

    //Sample Code
    private readonly List<GameObject> _elementCache = new();

    void OnEnable()
    {
        EventBus.Subscribe<BuildInteractionEvent>(OnBuildInteraction);
        view.SetActive(false);
    }

    void OnDisable() => EventBus.Unsubscribe<BuildInteractionEvent>(OnBuildInteraction);

    private void OnBuildInteraction(BuildInteractionEvent eventInfo)
    {
        if (eventInfo == null)
            return;

        if (!eventInfo.UIActive)
        {
            view.SetActive(false);
            return;
        }

        _elementCache.ForEach(s => Destroy(s));
        _elementCache.Clear();
        view.gameObject.SetActive(true);

        for(int i = 0; i < eventInfo.buildings.Count; i++)
        {
            var iter = eventInfo.buildings[i];
            var index = i;
            var go = Instantiate(buildInfoElementPrefab, elementParent.transform).GetComponent<BuildingElement>();

            go.gameObject.SetActive(true);

            go.SetElement(iter, eventInfo.inventory, () =>
            {
                eventInfo.onButtonEvent.Invoke(index);
            });

            _elementCache.Add(go.gameObject);
        }
    }

    public void OnUI(GameObject go)
    {
        view.SetActive(true);
    }
}
