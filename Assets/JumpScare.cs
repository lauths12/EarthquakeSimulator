using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    public GameObject jumpscareImage;   // La imagen del jumpscare
    public AudioSource jumpscareAudio;  // El sonido del grito

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisión detectada con: " + other.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("✅ Player detectado");

            if (jumpscareImage != null)
            {
                jumpscareImage.SetActive(true);
                Debug.Log("🎃 Jumpscare activado");
            }

            if (jumpscareAudio != null)
            {
                jumpscareAudio.Play(); // 🔊 Reproduce el grito
                Debug.Log("🔊 Sonido del jumpscare reproducido");
            }
        }
    }
}
