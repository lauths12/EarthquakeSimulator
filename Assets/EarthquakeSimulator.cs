using UnityEngine;

public class SismoGroundMoverMejorado : MonoBehaviour
{
    [Header("Parámetros del Sismo")]
    public float shakeMagnitude = 0.1f;    // Intensidad general del movimiento
    public float shakeSpeed = 5f;          // Velocidad de las vibraciones
    public float verticalRatio = 0.2f;     // Ratio de intensidad para el eje Y (20% del horizontal)

    private Vector3 originalPosition;

    void Start()
    {
        // Usamos position en lugar de localPosition, a menos que el suelo tenga un padre que también se mueva
        originalPosition = transform.position;
    }

    void FixedUpdate()
    {
        float time = Time.time * shakeSpeed;

        // --- Cálculo de Desplazamiento Horizontal (X y Z) ---

        // 1. Eje X: Usa el ruido Perlin con el tiempo
        float offsetX = Mathf.PerlinNoise(time, 0f) * 2f - 1f;

        // 2. Eje Z: Usa el ruido Perlin con un offset diferente para que no sea idéntico a X
        float offsetZ = Mathf.PerlinNoise(0f, time) * 2f - 1f;

        // --- Cálculo de Desplazamiento Vertical (Y) ---

        // 3. Eje Y: Usa un offset en el ruido Perlin diferente a X/Z y aplica el 'verticalRatio'
        float offsetY = (Mathf.PerlinNoise(time * 0.5f, time * 0.5f) * 2f - 1f) * verticalRatio;

        // --- Aplicación de Magnitud y Posición ---

        // Combina los offsets y aplica la magnitud
        Vector3 rawShakeOffset = new Vector3(offsetX, offsetY, offsetZ);
        Vector3 finalShakeOffset = rawShakeOffset * shakeMagnitude;

        // Aplica el desplazamiento al suelo
        transform.position = originalPosition + finalShakeOffset;
    }
}