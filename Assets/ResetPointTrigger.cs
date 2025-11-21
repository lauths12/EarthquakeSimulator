using UnityEngine;

public class ResetZoneTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Tu rig de VR debe tener este tag
        {
            Debug.Log("Jugador entró al tapete. Reiniciando...");
            FindObjectOfType<TimeManager>().ResetTimer();
        }
    }
}
