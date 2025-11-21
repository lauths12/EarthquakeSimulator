using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public enum ZoneType { Safe, Danger }
    public ZoneType zoneType;

    public float safePointsPerSecond = 2f;     // +2 por segundo
    public float dangerPointsPerSecond = 3f;   // -3 por segundo

    private bool playerInside = false;
    private InventoryManager inventory;
    private EarthquakePhysics quake;

    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        quake = FindObjectOfType<EarthquakePhysics>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }

    void Update()
    {
        // No sumar/restar si no hay terremoto
        if (!quake.isShaking) return;

        if (playerInside)
        {
            float amount = 0;

            if (zoneType == ZoneType.Safe)
                amount = safePointsPerSecond * Time.deltaTime;
            else
                amount = -dangerPointsPerSecond * Time.deltaTime;

            inventory.ModifyScoreContinuous(amount);
        }
    }
}
