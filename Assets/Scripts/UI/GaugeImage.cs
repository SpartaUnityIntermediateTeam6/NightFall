using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//두트윈 적용시 변경

public class GaugeImage : MonoBehaviour
{
    [SerializeField] private Image gaugeImage;

    void Awake()
    {
        if (!gaugeImage)
        {
            gaugeImage = GetComponent<Image>();
        }
    }

    public void Changed(float current, float max)
    {

    }

    public void Changed(BoundedValue value)
    {
        gaugeImage.fillAmount = value.Ratio;
    }
}
