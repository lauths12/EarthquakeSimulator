using UnityEngine;
using TMPro;
using System.Collections;

public class KeyManager : MonoBehaviour
{
    public int totalKeys = 3;
    private int collectedKeys = 0;

    public TextMeshProUGUI keysText;
    public float messageDuration = 2f;

    private float lastKeyTime = 0f;   
    private float minInterval = 0.2f; 

    void Start()
    {
        if (keysText != null)
            keysText.gameObject.SetActive(false);
    }

    public void CollectKey()
    {
     
        if (Time.time - lastKeyTime < minInterval)
            return;

        lastKeyTime = Time.time;

        collectedKeys++;
        if (keysText != null)
        {
            keysText.gameObject.SetActive(true);
            UpdateUI();

            StopAllCoroutines();
            StartCoroutine(HideTextAfterDelay());
        }

        if (collectedKeys >= totalKeys)
        {
            WinGame();
        }
    }

    private void UpdateUI()
    {
        if (keysText != null)
            keysText.text = "Llaves: " + collectedKeys + "/" + totalKeys;
    }

    private void WinGame()
    {
        if (keysText != null)
        {
            keysText.gameObject.SetActive(true);
            keysText.text = "¡Ganaste el juego!";
        }
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        if (collectedKeys < totalKeys && keysText != null)
            keysText.gameObject.SetActive(false);
    }
}
