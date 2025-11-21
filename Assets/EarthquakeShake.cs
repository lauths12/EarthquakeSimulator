using UnityEngine;

public class EarthquakeShake : MonoBehaviour
{
    [Header("Earthquake Settings")]
    public bool isShaking = false;
    public float intensity = 0.03f;     // Cantidad de movimiento (5 cm aprox)
    public float speed = 20f;           // Velocidad del ruido -> sismo fuerte

    private Vector3 originalPos;

    void Start()
    {
        // Guardamos la posición original de la cámara
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (!isShaking)
            return;

        // Perlin noise para movimiento suave e irregular
        float x = (Mathf.PerlinNoise(Time.time * speed, 0f) - 0.5f) * intensity;
        float y = (Mathf.PerlinNoise(0f, Time.time * speed) - 0.5f) * intensity * 0.4f; // menos Y
        float z = (Mathf.PerlinNoise(Time.time * speed, Time.time * 0.1f) - 0.5f) * intensity;

        transform.localPosition = originalPos + new Vector3(x, y, z);
    }

    public void StartEarthquake()
    {
        isShaking = true;
    }

    public void StopEarthquake()
    {
        isShaking = false;
        transform.localPosition = originalPos;
    }
}
