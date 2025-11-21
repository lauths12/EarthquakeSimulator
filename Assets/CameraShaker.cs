using UnityEngine;

public class CameraShakerFixed : MonoBehaviour
{
    [Header("Parámetros del Sismo en Cámara")]
    [Tooltip("Magnitud del movimiento de posición (ej. 0.05 a 0.2)")]
    public float positionMagnitude = 0.15f;

    [Tooltip("Magnitud del movimiento de rotación en grados (ej. 2 a 5)")]
    public float rotationMagnitude = 3f;

    [Tooltip("Velocidad de las vibraciones (ej. 8 a 12)")]
    public float shakeSpeed = 10f;

    private Vector3 originalPos;
    private Quaternion originalRot;

    void Start()
    {
        // Guardamos la posición y rotación global de la cámara
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    // Usar Update para un movimiento visual más fluido
    void Update()
    {
        float time = Time.time * shakeSpeed;

        // --- 1. Posición (Movimiento sutil) ---

        // Movimiento en X y Z
        float offsetX = (Mathf.PerlinNoise(time, 0f) * 2f - 1f) * positionMagnitude;
        float offsetZ = (Mathf.PerlinNoise(0f, time) * 2f - 1f) * positionMagnitude;

        // Movimiento en Y (vertical), generalmente más sutil, por eso el * 0.5f
        float offsetY = (Mathf.PerlinNoise(time + 10f, 0f) * 2f - 1f) * positionMagnitude * 0.5f;

        Vector3 posOffset = new Vector3(offsetX, offsetY, offsetZ);
        transform.position = originalPos + posOffset;

        // --- 2. Rotación (Genera la sensación de mareo/inmersión) ---

        // Rotación en X y Y
        float rotX = (Mathf.PerlinNoise(time + 5f, 0f) * 2f - 1f) * rotationMagnitude;
        float rotY = (Mathf.PerlinNoise(0f, time + 15f) * 2f - 1f) * rotationMagnitude;

        // Aplicamos la rotación como un desplazamiento (offset)
        Quaternion rotOffset = Quaternion.Euler(rotX, rotY, 0f);

        // Combinamos la rotación original con el desplazamiento
        transform.rotation = originalRot * rotOffset;
    }
}