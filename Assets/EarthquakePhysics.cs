using UnityEngine;

public class EarthquakePhysics : MonoBehaviour
{
    public float forceAmount = 10f;
    public float torqueAmount = 5f;
    public bool isShaking = false;

    Rigidbody[] allBodies;

    void Start()
    {
        allBodies = FindObjectsOfType<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isShaking) return;

        foreach (var rb in allBodies)
        {
            if (rb.isKinematic) continue;

            Vector3 randomForce = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-0.2f, 0.5f),
                Random.Range(-1f, 1f)
            ) * forceAmount;

            rb.AddForce(randomForce, ForceMode.Acceleration);

            Vector3 randomTorque = Random.insideUnitSphere * torqueAmount;
            rb.AddTorque(randomTorque, ForceMode.Acceleration);
        }
    }

    public void StartEarthquake()
    {
        isShaking = true;
    }

    public void StopEarthquake()
    {
        isShaking = false;
    }
}
