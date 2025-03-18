using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect; // 스크롤 뷰의 ScrollRect 컴포넌트
    [SerializeField] private float scrollSpeed = 0.1f; // 자동 스크롤 속도
    [SerializeField] private string startSceneName = "StartScene"; // 스타트 씬 이름

    private bool isReturning = false;

    private void Update()
    {
        // 자동 스크롤
        if (scrollRect.verticalNormalizedPosition > 0)
        {
            scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;
        }
        else if (!isReturning) // 스크롤이 끝까지 내려갔을 때
        {
            isReturning = true;
            Invoke("ReturnToStartScene", 1f); // 1초 후에 씬 전환
        }
    }

    private void ReturnToStartScene()
    {
        SceneManager.LoadScene(startSceneName);
    }
}



