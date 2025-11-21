using UnityEngine;

public class FacePlayerOnY : MonoBehaviour
{
    public Transform playerCamera;

    void Update()
    {
        Vector3 dir = playerCamera.position - transform.position;
        dir.y = 0;
        transform.forward = dir;
    }
}
