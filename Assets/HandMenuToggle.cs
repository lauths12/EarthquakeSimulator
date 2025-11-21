using UnityEngine;
using Valve.VR;

public class HandMenuToggle : MonoBehaviour
{
    public GameObject handMenu; // Tu canvas / menú VR
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Action_Boolean menuAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("MenuClick");

    private bool isOpen = false;

    void Start()
    {
        // El menú siempre inicia oculto
        handMenu.SetActive(false);
    }

    void Update()
    {
        if (menuAction != null && menuAction.GetStateDown(hand))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isOpen = !isOpen;
        handMenu.SetActive(isOpen);

        Debug.Log("Menú ahora: " + (isOpen ? "ABIERTO" : "CERRADO"));
    }
}
