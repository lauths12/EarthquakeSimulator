using UnityEngine;
using Valve.VR;

public class HandCrouchVirtual : MonoBehaviour
{
    [Header("SteamVR")]
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Action_Boolean crouchAction =
        SteamVR_Input.GetAction<SteamVR_Action_Boolean>("MenuClick");

    [Header("References")]
    public CharacterController cc;

    // Alturas por las que quieres ir ciclando: 0.5 -> 1.5 -> 2 -> 2.99 -> 0.5 ...
    [Header("Heights")]
    public float[] heights = { 0.5f, 1.5f, 2f, 2.99f };

    // Índice actual dentro del array
    private int currentIndex = 3; // 3 = 2.99f (último elemento)

    void Start()
    {
        if (cc == null)
            cc = GetComponent<CharacterController>();

        // Comenzamos de pie en 2.99
        cc.height = heights[currentIndex];
    }

    void Update()
    {
        if (crouchAction != null && crouchAction.GetStateDown(hand))
        {
            CycleHeight();
        }
    }

    void CycleHeight()
    {
        // Avanzamos al siguiente valor: 0.5 -> 1.5 -> 2 -> 2.99 -> ...
        currentIndex = (currentIndex + 1) % heights.Length;

        float targetHeight = Mathf.Clamp(heights[currentIndex], 0.5f, 3.0f);

        cc.height = targetHeight;

        Debug.Log($"[CROUCH] Height={targetHeight}");
    }
}
