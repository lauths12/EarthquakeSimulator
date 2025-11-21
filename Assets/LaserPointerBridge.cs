using UnityEngine;
using Valve.VR.Extras;  

public class LaserPointerBridge : MonoBehaviour
{
    public SteamVR_LaserPointer laser;

    void Awake()
    {
        if (laser == null) laser = GetComponent<SteamVR_LaserPointer>();
        laser.PointerIn += OnPointerIn;
        laser.PointerOut += OnPointerOut;
        laser.PointerClick += OnPointerClick;
    }
    void OnDestroy()
    {
        if (laser == null) return;
        laser.PointerIn -= OnPointerIn;
        laser.PointerOut -= OnPointerOut;
        laser.PointerClick -= OnPointerClick;
    }

    void OnPointerIn(object sender, PointerEventArgs e)
    {
        e.target.SendMessage("VRHoverEnter", SendMessageOptions.DontRequireReceiver);
    }
    void OnPointerOut(object sender, PointerEventArgs e)
    {
        e.target.SendMessage("VRHoverExit", SendMessageOptions.DontRequireReceiver);
    }
    void OnPointerClick(object sender, PointerEventArgs e)
    {
        e.target.SendMessage("VRClick", SendMessageOptions.DontRequireReceiver);
    }
}
