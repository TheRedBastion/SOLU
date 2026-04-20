using UnityEngine;
using UnityEngine.Rendering.Universal;

public class IntensityChanger : MonoBehaviour
{
    [Header("Intensity Settings")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 2f;
    [SerializeField] private float timePeriod = 2f;

    [Header("Optional")]
    [SerializeField] private bool useSmoothSine = true;

    private Light2D light2D;
    private float startTime;

    void Start()
    {
        light2D = GetComponent<Light2D>();

        if (light2D == null)
        {
            Debug.LogError("Light2D not found");
            enabled = false;
            return;
        }

        startTime = Time.time;
    }

    void Update()
    {
        if (light2D == null) return;

        float t;

        if (useSmoothSine)
        {
            //sine wave
            t = Mathf.Sin((Time.time - startTime) * (2f * Mathf.PI / timePeriod)) * 0.5f + 0.5f;
        }
        else
        {
            //lerp
            t = Mathf.PingPong((Time.time - startTime) * (2f / timePeriod), 1f);
        }

        light2D.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
    }
}
