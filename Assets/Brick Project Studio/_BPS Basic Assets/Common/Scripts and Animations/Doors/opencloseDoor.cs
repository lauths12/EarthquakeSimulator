using System.Collections;
using UnityEngine;

namespace SojaExiles
{
    public class OpenCloseDoorVR : MonoBehaviour
    {
        public Animator openAndClose;
        public bool open = false;

        [Tooltip("Distancia mínima del jugador para abrir la puerta")]
        public float maxDistance = 15f;

        public Transform player;

        // Se dispara al entrar cualquier dedo
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PlayerHand") && player != null && !open)
            {
                float dist = Vector3.Distance(player.position, transform.position);
                if (dist <= maxDistance)
                {
                    StartCoroutine(Opening());
                }
            }
        }

        // Se dispara al salir cualquier dedo
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PlayerHand") && player != null && open)
            {
                float dist = Vector3.Distance(player.position, transform.position);
                if (dist <= maxDistance)
                {
                    StartCoroutine(Closing());
                }
            }
        }

        IEnumerator Opening()
        {
            Debug.Log("You are opening the door");
            openAndClose.Play("Opening");
            open = true;
            yield return new WaitForSeconds(0.5f); // Tiempo de animación
        }

        IEnumerator Closing()
        {
            Debug.Log("You are closing the door");
            openAndClose.Play("Closing");
            open = false;
            yield return new WaitForSeconds(0.5f); // Tiempo de animación
        }
    }
}
