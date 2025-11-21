using TMPro;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class DoorExitZone : MonoBehaviour
{
    [Header("SteamVR Input")]
    public SteamVR_Action_Boolean grabPinchAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.Any;

    [Header("UI")]
    public TextMeshProUGUI exitText;

    [Header("Earthquake Reference")]
    public EarthquakePhysics earthquake;

    [Header("Scoring Reference")]
    public InventoryManager inventoryManager;

    [Header("Time Reference")]
    public TimeManager timeManager;   // 🔹 referencia al TimeManager

    private bool playerInside = false;

    private void Start()
    {
        if (exitText != null)
            exitText.text = "Salida";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = true;

        // 🔹 Solo mostramos el mensaje si se cumple alguna condición de salida
        if (!CanExit())
            return;

        exitText.text = "Presiona el gatillo para salir";
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        exitText.text = "";
    }

    private void Update()
    {
        if (!playerInside) return;

        // 🔹 Comprobar continuamente si aún se puede salir
        if (!CanExit())
        {
            exitText.text = "";
            return;
        }

        if (exitText.text == "")
            exitText.text = "<size=60%>Presiona el gatillo para salir</size>";

        if (grabPinchAction != null && grabPinchAction.GetStateDown(hand))
        {
            EndSimulation();
        }
    }

    // 🔹 Lógica de “¿se permite salir?”
    private bool CanExit()
    {
        bool isShaking = (earthquake != null && earthquake.isShaking);

        bool timeUpOtherDay = (timeManager != null &&
                               timeManager.IsTimeUp() &&
                               timeManager.currentDay != 1);

        // Se puede salir si tiembla o si el tiempo llegó a 0 en un día distinto de 1
        return isShaking || timeUpOtherDay;
    }

    private void EndSimulation()
    {
        exitText.text = "<color=yellow><size=60%><b>Simulacro terminado</b></size></color>";

        Debug.Log("SIMULACRO TERMINADO");

        if (inventoryManager != null)
        {
            int totalScore = inventoryManager.itemScore
                           + Mathf.RoundToInt(inventoryManager.zoneScoreFloat);

            Debug.Log("===== RESULTADOS DEL SIMULACRO =====");
            Debug.Log("Puntaje Objetos (Día 1): " + inventoryManager.itemScore);
            Debug.Log("Puntaje Zonas (Día 2): " + inventoryManager.zoneScoreFloat);
            Debug.Log("PUNTAJE TOTAL: " + totalScore);
        }

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
