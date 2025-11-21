using System.Collections;
using System.Runtime.ConstrainedExecution;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public const float TIME_LIMIT = 30f;

    public TextMeshProUGUI timerText;
    private float currentTime;
    private bool isTimeUp = false;

    public InventoryManager inventoryManager;

    [Header("Reset Point")]
    public GameObject resetPoint;
    public ScreenFade screenFade;

    [Header("Earthquake System")]
    public EarthquakePhysics earthquakePhysics;
    public EarthquakeShake earthquakeShake;
    public int currentDay = 1;             // Día actual
    private bool earthquakeStarted = false;

    private float halfTime;                // Momento exacto para activar
    private Coroutine timerRoutine;

    [Header("Time Panel UI")]
    public RectTransform timePanelRect;         // La Image que hace de fondo
    public Vector2 normalSize = new Vector2(200f, 80f);
    public Vector2 quakeSize = new Vector2(450f, 200f);

    void Start()
    {
        currentTime = TIME_LIMIT;
        halfTime = TIME_LIMIT / 2f;

        inventoryManager = FindObjectOfType<InventoryManager>();

        resetPoint.SetActive(false);

        if (timerText != null) timerText.gameObject.SetActive(true);

        // Si no asignaste el panel en el inspector, tomo el padre del texto
        if (timePanelRect == null && timerText != null)
        {
            timePanelRect = timerText.transform.parent.GetComponent<RectTransform>();
        }

        // Guardar el tamaño inicial real del panel
        if (timePanelRect != null)
        {
            normalSize = timePanelRect.sizeDelta;
        }

        timerRoutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        while (currentTime > 0)
        {
            yield return null;
            currentTime -= Time.deltaTime;

            // Día 2 o más → terremoto a mitad del tiempo
            if (currentDay >= 2 && !earthquakeStarted && currentTime <= halfTime)
            {
                earthquakePhysics.StartEarthquake();
                earthquakeShake.StartEarthquake();
                earthquakeStarted = true;
                Debug.Log("🌋 TERREMOTO ACTIVADO (Día " + currentDay + ")");
            }

            UpdateTimerUI();
        }

        // Tiempo terminó
        currentTime = 0;
        isTimeUp = true;
        UpdateTimerUI();

        // Desactivar picking
        if (inventoryManager != null)
            inventoryManager.DisablePicking = true;

        // Detener terremoto si es Día 2 o más
        if (currentDay >= 2)
        {
            earthquakePhysics.StopEarthquake();
            earthquakeShake.StopEarthquake();
            Debug.Log("TERREMOTO DETENIDO (Tiempo terminado, Día " + currentDay + ")");
        }

        if (currentDay == 1)
            resetPoint.SetActive(true);

        Debug.Log("⏳ Tiempo terminado. Tapete habilitado.");
    }

    public void ResetTimer()
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        // Pantalla negra
        yield return screenFade.FadeOut(10f);

        // Siguiente día
        currentDay++;
        earthquakeStarted = false;
        earthquakePhysics.StopEarthquake();
        earthquakeShake.StopEarthquake();

        // Reiniciar valores
        currentTime = TIME_LIMIT;
        halfTime = TIME_LIMIT / 2f;
        isTimeUp = false;
        inventoryManager.DisablePicking = false;

        resetPoint.SetActive(false);

        UpdateTimerUI();

        // Fade de regreso
        yield return screenFade.FadeIn(10f);

        // Reiniciar conteo
        timerRoutine = StartCoroutine(CountdownRoutine());
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        // Si está temblando, mostramos el mensaje de decisión
        if (earthquakePhysics != null && earthquakePhysics.isShaking)
        {
            // Hacer el panel más grande mientras tiembla
            if (timePanelRect != null)
                timePanelRect.sizeDelta = quakeSize;

            timerText.text =
                "<b><size=100%>¡Sismo en progreso!</size></b>\n" +
                "<size=80%>Elige cómo actuar:\n" +
                "• Refúgiate en una zona segura dentro de la vivienda. (lejos de ventanas y objetos que puedan caer). \n" +
                "• Evacúa de inmediato hacia una zona segura en el exterior.</size>";

            return;
        }

        // Si NO está temblando, devolvemos el panel a su tamaño norma
        if (timePanelRect != null)
            timePanelRect.sizeDelta = normalSize;

        // Mostrar el tiempo / mensajes normales
        string minutes = Mathf.Floor(currentTime / 60).ToString("00");
        string seconds = Mathf.Floor(currentTime % 60).ToString("00");

        if (isTimeUp)
        {
            if (currentDay == 1)
            {
                // Mensaje para el Día 1
                timerText.text = "<size=60%><color=#9B59FF><b>¡Ve a la cama!</b></color></size>";
            }
            else
            {
                timerText.text = "<color=red><b>00:00</b></color>";
            }
        }
        else
        {
            timerText.text = $"<b>{minutes}:{seconds}</b>";
        }
    }
    public bool IsTimeUp() => isTimeUp;
}
