using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreamManager : MonoBehaviour
{
    [Header("Referencias")]
    public CanvasGroup screamCanvasGroup; // CanvasGroup para controlar alpha
    public AudioSource screamAudio;       // Sonido del scream

    [Header("Configuración")]
    public float fadeInSpeed = 3f;        // Velocidad de aparición
    public float holdTime = 2f;          // Tiempo que permanece visible
    public float fadeOutSpeed = 2f;      // Velocidad de desaparición

    private bool isScreaming = false;

    public void ShowScream()
    {
        if (!isScreaming)
            StartCoroutine(ScreamEffect());
    }

    private IEnumerator ScreamEffect()
    {
        isScreaming = true;

        // Reproducir sonido
        if (screamAudio != null)
            screamAudio.Play();

        // Fade IN
        while (screamCanvasGroup.alpha < 1f)
        {
            screamCanvasGroup.alpha += Time.deltaTime * fadeInSpeed;
            yield return null;
        }

        // Mantenerlo visible un tiempo
        yield return new WaitForSeconds(holdTime);

        // Fade OUT
        while (screamCanvasGroup.alpha > 0f)
        {
            screamCanvasGroup.alpha -= Time.deltaTime * fadeOutSpeed;
            yield return null;
        }

        isScreaming = false;
    }
}
