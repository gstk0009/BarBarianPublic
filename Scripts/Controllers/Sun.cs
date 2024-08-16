using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class Sun : MonoBehaviour
{

    public Light2D sunlight;
    public float nightIntensity;
    public float dayIntensity;

    public AnimationCurve dayNightCurve;
    public Action OnDateTimeChanged;

    private void OnEnable()
    {
        ClockSystem.OnTimeChanged += UpdateLight;
    }

    public void UpdateLight()
    {
        float t = (float)ClockSystem.Hour / 24f;

        float dayNightT = dayNightCurve.Evaluate(t);
        sunlight.intensity = Mathf.Lerp(nightIntensity, dayIntensity, dayNightT);
    }
}
