using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScroll : MonoBehaviour
{
    public RectTransform creditText; // 크레딧 텍스트의 RectTransform
    public float scrollSpeed = 50f;  // 스크롤 속도

    private float startY;
    private float endY;

    void Start()
    {
        startY = creditText.anchoredPosition.y;
        endY = startY + 1500; // 크레딧 끝나는 위치 조절
    }

    void Update()
    {
        if (creditText.anchoredPosition.y < endY)
        {
            creditText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        }
    }
}

