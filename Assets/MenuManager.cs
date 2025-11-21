using UnityEngine;
using Valve.VR.Extras; // SteamVR_LaserPointer

public class MenuManager : MonoBehaviour
{
    [Header("Raíces")]
    public GameObject menuRoot;
    public GameObject gameplayRoot;

    [Header("VR")]
    public SteamVR_LaserPointer rightHandLaser; // arrastra el componente de RightHand

    void Awake()
    {
        // Mostrar menú, ocultar gameplay, encender láser
        if (menuRoot) menuRoot.SetActive(true);
        if (gameplayRoot) gameplayRoot.SetActive(false);
        if (rightHandLaser) rightHandLaser.enabled = true;

        // (Opcional) Pausar el tiempo si tienes animaciones sueltas en Update
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        if (menuRoot) menuRoot.SetActive(false);
        if (gameplayRoot) gameplayRoot.SetActive(true);
        if (rightHandLaser) rightHandLaser.enabled = false;

        Time.timeScale = 1f; // reanudar
    }
}
