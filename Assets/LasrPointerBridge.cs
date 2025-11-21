using UnityEngine;
using Valve.VR;
using Valve.VR.Extras;

public class LaserPointerClicker : MonoBehaviour
{
    public SteamVR_LaserPointer laser;
    public SteamVR_Behaviour_Pose pose;
    // Usa la acción que mapea el TRIGGER (normalmente /actions/default/in/InteractUI)
    public SteamVR_Action_Boolean clickAction = SteamVR_Input.GetBooleanAction("InteractUI");
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.RightHand;

    private VRStartButton current; // objetivo bajo el láser

    void Awake()
    {
        if (!laser) laser = GetComponent<SteamVR_LaserPointer>();
        if (!pose) pose = GetComponent<SteamVR_Behaviour_Pose>();
        laser.PointerIn += OnPointerIn;
        laser.PointerOut += OnPointerOut;
        laser.PointerClick += OnPointerClick; // por si tu versión sí lo envía
    }
    void OnDestroy()
    {
        if (!laser) return;
        laser.PointerIn -= OnPointerIn;
        laser.PointerOut -= OnPointerOut;
        laser.PointerClick -= OnPointerClick;
    }

    void Update()
    {
        // fallback robusto: si el trigger se presiona, disparamos el click en el objetivo actual
        if (current != null && clickAction != null && clickAction.GetStateDown(hand))
        {
            Debug.Log("[Laser] Trigger -> Click sobre " + current.name);
            current.OnClick();
        }
    }

    void OnPointerIn(object sender, PointerEventArgs e)
    {
        current = e.target.GetComponentInParent<VRStartButton>();
        if (current != null) current.OnHoverEnter();
    }

    void OnPointerOut(object sender, PointerEventArgs e)
    {
        var btn = e.target.GetComponentInParent<VRStartButton>();
        if (btn != null) btn.OnHoverExit();
        if (btn == current) current = null;
    }

    // Si tu LaserPointer sí emite PointerClick, también lo atendemos:
    void OnPointerClick(object sender, PointerEventArgs e)
    {
        var btn = e.target.GetComponentInParent<VRStartButton>();
        if (btn != null)
        {
            Debug.Log("[Laser] PointerClick -> " + btn.name);
            btn.OnClick();
        }
    }
}
