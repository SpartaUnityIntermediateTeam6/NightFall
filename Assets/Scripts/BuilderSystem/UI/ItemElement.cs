using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemElement : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI amountTmp;

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public void SetText(string text)
    {
        amountTmp.text = text;
    }
}
