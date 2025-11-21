using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VRStartButton : MonoBehaviour
{
    public GameObject menuRoot;
    public GameObject gameplayRoot;

    public void OnHoverEnter() { /* opcional: feedback visual */ }
    public void OnHoverExit() { /* opcional: feedback visual */ }

    public void OnClick()
    {
        Debug.Log("VRStartButton -> CLICK (empezar juego)");
        if (menuRoot) menuRoot.SetActive(false);
        if (gameplayRoot) gameplayRoot.SetActive(true);
        Time.timeScale = 1f; // por si pausaste el menú
        // apaga el láser si no lo quieres en gameplay:
        var laser = FindObjectOfType<Valve.VR.Extras.SteamVR_LaserPointer>();
        if (laser) laser.enabled = false;
    }
}
