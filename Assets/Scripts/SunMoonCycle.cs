using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class SunMoonCycle : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public RectTransform timeArrow; // UI 화살표의 RectTransform
    public RectTransform sunMoonTimer;
    public RectTransform bigSunMoonTimer;
    public RectTransform bigTimeArrow;
    public Material skyBoxMaterial;

    public float cycleDelay;

    public event Action dayNight;

    public int dDay;

    [Range(0.0f, 1f)]
    public float time;
    public float startTime;
    private float timeRate;

    public bool isNextDay;
    public bool isNight;
    public bool isRote;
    public float sunTime;
    public float moonTime;
    private float rotation;
    private float originalSkyColor;
    private Color originalGroundColor;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;
    public AnimationCurve skyBoxCurve;

    void Start()
    {
        originalSkyColor = RenderSettings.skybox.GetFloat("_AtmosphereThickness");
        originalGroundColor = skyBoxMaterial.GetColor("_GroundColor"); // 실행 전 원래 값 저장
        time = startTime;
    }

    void Update()
    {
        if (!isNight) timeRate = 1.0f / sunTime;
        else timeRate = 1.0f / moonTime;

        if (!isRote)
        {
            time += (timeRate * Time.deltaTime) / 2;

            rotation = isNight ? (2 * (time - 0.5f) * 180.0f) : (2 * time * 180.0f);  // time 값(0~1)을 0~180도로 변환

            if ((!isNight && time >= 0.5f) || (isNight && time >= 1f))
                rotation = 180f;
            timeArrow.rotation = Quaternion.Euler(0, 0, -rotation); // 시계방향 회전
        }


        if (rotation == 180f)
        {
            DayNightUpdate();
        }


        UpdateLighting(sun, sunColor, sunIntensity);
        sun.intensity = sunIntensity.Evaluate(time);

        if (UnityEngine.ColorUtility.TryParseHtmlString(isNight ? "#000000" : "#3D7FB6", out Color groundColor))
        {
            skyBoxMaterial.SetColor("_GroundColor", groundColor);
        }

        RenderSettings.skybox.SetFloat("_AtmosphereThickness", skyBoxCurve.Evaluate(time));  // 시간에 따른 skybox 빛 반사율
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.ambientIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }


    public void DayNightUpdate()
    {
        rotation = 0f;
        isRote = true;
        isNight = !isNight;
        time = isNight ? 0.5f : 0;

        dayNight?.Invoke();

        bigSunMoonTimer.gameObject.SetActive(true);
        bigTimeArrow.gameObject.SetActive(true);

        bigSunMoonTimer.DOLocalRotate(bigSunMoonTimer.transform.eulerAngles + new Vector3(0, 0, 180f), cycleDelay).SetEase(Ease.InOutQuad).OnComplete(() =>
        {
            bigSunMoonTimer.gameObject.SetActive(false);
            bigTimeArrow.gameObject.SetActive(false);
            sunMoonTimer.Rotate(0, 0, 180f);
            isRote = false;
        });

        if (!isNight) dayText.text = (++dDay).ToString();
    }




    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        if (isNight) lightSource.color = gradient.Evaluate(time);
        else lightSource.color = gradient.Evaluate(time - 0.5f);

        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }

    void OnDestroy()
    {
        // 실행 종료 시 원래 값으로 복원
        RenderSettings.skybox.SetFloat("_AtmosphereThickness", originalSkyColor);
        skyBoxMaterial.SetColor("_GroundColor", originalGroundColor);
    }

}
