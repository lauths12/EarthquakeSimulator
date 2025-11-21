using UnityEngine;
using Valve.VR;

public class LobbyTransition : MonoBehaviour
{
    [Header("VR Input")]
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.RightHand;
    public SteamVR_Action_Boolean triggerAction = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("GrabPinch");

    [Header("Scene References")]
    public GameObject lobby;
    public GameObject structure03;

    private bool hasTransitioned = false; // para que solo ocurra una vez

    void Start()
    {
        // Estado inicial
        lobby.SetActive(true);
        structure03.SetActive(false);
    }

    void Update()
    {
        if (!hasTransitioned && triggerAction.GetStateDown(hand))
        {
            lobby.SetActive(false);
            structure03.SetActive(true);
            hasTransitioned = true;

            Debug.Log("Transición a Structure_03 completada");
        }
    }
}
