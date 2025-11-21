using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class PickupItemVR : MonoBehaviour
{
    public string itemTag = "Food";

    // Define la acción de SteamVR para guardar el ítem
    public SteamVR_Action_Boolean activateInventoryAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabGrip");

    private Interactable interactable;
    private Hand currentHoldingHand; // Referencia a la mano que lo sostiene
    private bool isHeld = false; // Bandera para saber si el objeto está en la mano

    void Start()
    {
        interactable = GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.onAttachedToHand += OnAttachedToHand;
            interactable.onDetachedFromHand += OnDetachedFromHand;
        }
    }

    void Update()
    {
        // 1. Verificar si el objeto está actualmente sostenido por una mano.
        if (isHeld && currentHoldingHand != null)
        {
            // 2. Usar GetStateDown para verificar si el botón fue presionado UNA VEZ este frame.
            if (activateInventoryAction != null && activateInventoryAction.GetStateDown(currentHoldingHand.handType))
            {
                // El botón fue presionado mientras sosteníamos el objeto, asi que se intenta guardar
                TryAddItemToInventory();
            }
        }
    }

    // --- Métodos de Interacción VR ---

    private void OnAttachedToHand(Hand hand)
    {
        isHeld = true;
        currentHoldingHand = hand;
    }

    private void OnDetachedFromHand(Hand hand)
    {
        isHeld = false;
        currentHoldingHand = null;
    }

    // --- Lógica del Inventario ---

    private void TryAddItemToInventory()
    {
        // 1. Buscar el InventoryManager
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();

        if (inventoryManager != null)
        {
            // 2. Intentar añadir el objeto.
            bool itemAdded = inventoryManager.AddItem(itemTag, gameObject);

            if (itemAdded)
            {
                // 3. ¡Éxito! Se guarda y se destruye del mundo.
                // El sistema VR lo limpiará automáticamente de la mano al destruirse.
                Destroy(gameObject);
            }
            else
            {
                // 4. Fallo (Inventario Lleno). 
                // La UI ya muestra "Inventario lleno".
                // Se deja el objeto en la mano del jugador para que él decida soltarlo.
            }
        }
    }
}