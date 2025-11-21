using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class ThrowableObject : MonoBehaviour
{
    [Header("Detecci�n de aterrizaje")]
    public bool triggerOnRest = true;     // true: espera a que el objeto se detenga
    public float restSpeed = 0.05f;       // velocidad para considerar �quieto�
    public float restTime = 0.2f;         // tiempo m�nimo quieto
    public float maxWaitToRest = 3f;      // espera m�x. tras primer choque

    [Header("Notificaci�n")]
    public bool onlyNearestEnemy = true;  // solo el enemigo m�s cercano
    public string enemyTag = "Enemy";     // tag de los enemigos (opcional)
    public bool debugLogs = false;

    private Rigidbody rb;
    private bool armed = false;       // se �arma� tras 1 frame para evitar falsos choques
    private bool notified = false;    // ya notific� una vez

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        notified = false;
        StartCoroutine(ArmNextFrame());
    }

    IEnumerator ArmNextFrame()
    {
        yield return null; // espera 1 frame para evitar choque al instanciar
        armed = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!armed || notified) return;

        if (debugLogs) Debug.Log($"[ThrowableObject] Colisi�n con: {collision.collider.name}");

        if (!triggerOnRest)
        {
            Vector3 point = collision.contacts.Length > 0 ? collision.contacts[0].point : transform.position;
            NotifyEnemies(point);
            notified = true;
        }
        else
        {
            // empezamos a esperar a que el objeto se quede quieto
            StartCoroutine(WaitUntilRestThenNotify());
        }
    }

    // Soporte por si tuvieras colliders como trigger
    void OnTriggerEnter(Collider other)
    {
        if (!armed || notified) return;

        if (debugLogs) Debug.Log($"[ThrowableObject] Trigger con: {other.name}");

        if (!triggerOnRest)
        {
            NotifyEnemies(transform.position);
            notified = true;
        }
        else
        {
            StartCoroutine(WaitUntilRestThenNotify());
        }
    }

    IEnumerator WaitUntilRestThenNotify()
    {
        float t = 0f;
        float stillFor = 0f;

        while (t < maxWaitToRest)
        {
            t += Time.deltaTime;

            if (rb != null && rb.linearVelocity.sqrMagnitude < restSpeed * restSpeed)
            {
                stillFor += Time.deltaTime;
                if (stillFor >= restTime) break;
            }
            else
            {
                stillFor = 0f;
            }

            yield return null;
        }

        if (!notified)
        {
            if (debugLogs) Debug.Log("[ThrowableObject] Notificando posici�n final.");
            NotifyEnemies(transform.position);
            notified = true;
        }
    }

    void NotifyEnemies(Vector3 pos)
    {
        if (onlyNearestEnemy)
        {
            EnemyAI nearest = FindNearestEnemy(pos);
            if (nearest != null) nearest.GoToObject(pos);
        }
        else
        {
            foreach (var enemy in FindObjectsOfType<EnemyAI>())
                enemy.GoToObject(pos);
        }
    }

    EnemyAI FindNearestEnemy(Vector3 pos)
    {
        EnemyAI nearest = null;
        float bestDist = float.PositiveInfinity;

        // Si usas tag en tus enemigos, esto es m�s r�pido/limpio
        var tagged = GameObject.FindGameObjectsWithTag(enemyTag);
        if (tagged != null && tagged.Length > 0)
        {
            foreach (var go in tagged)
            {
                var ai = go.GetComponent<EnemyAI>();
                if (ai == null) continue;
                float d = (go.transform.position - pos).sqrMagnitude;
                if (d < bestDist) { bestDist = d; nearest = ai; }
            }
        }
        else
        {
            // Fallback si no usas tag
            foreach (var ai in FindObjectsOfType<EnemyAI>())
            {
                float d = (ai.transform.position - pos).sqrMagnitude;
                if (d < bestDist) { bestDist = d; nearest = ai; }
            }
        }

        return nearest;
    }
}
