using UnityEngine;
using Valve.VR.InteractionSystem;

public class KeyVR : MonoBehaviour
{
    private Throwable throwable;
    private bool collected = false; 

    void Start()
    {
        throwable = GetComponent<Throwable>();
        if (throwable != null)
        {
            throwable.onPickUp.AddListener(OnPickedUp);
        }
    }

    private void OnPickedUp()
    {
        if (collected) return; 
        collected = true;


        KeyManager keyManager = FindObjectOfType<KeyManager>();
        if (keyManager != null)
        {
            keyManager.CollectKey();
        }

        Destroy(gameObject, 1f);
    }
}
