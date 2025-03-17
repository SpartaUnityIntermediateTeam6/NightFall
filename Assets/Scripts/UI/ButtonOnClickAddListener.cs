using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonOnClickAddListener : MonoBehaviour
{
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void AddListener(UnityAction addEvent)
    {
        if (addEvent != null)
        {
            _button.onClick.AddListener(addEvent);
        }
    }

    public void RemoveAllAndAdd(UnityAction addEvent)
    {
        //_button.onClick.RemoveAllListeners();
        //AddListener(addEvent);
    }
}
