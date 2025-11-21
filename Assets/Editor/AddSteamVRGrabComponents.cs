using UnityEditor;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AddSteamVRGrabComponents
{
    [MenuItem("Tools/Add SteamVR Grab & Throw To All Rigidbodies")]
    static void AddGrabAndThrowToRigidbodies()
    {
        Rigidbody[] allRigidbodies = GameObject.FindObjectsOfType<Rigidbody>();
        int addedCount = 0;

        // Load the sphereSmallPose
        SteamVR_Skeleton_Pose sphereSmallPose = AssetDatabase.LoadAssetAtPath<SteamVR_Skeleton_Pose>(
            "Assets/SteamVR/InteractionSystem/Poses/sphereSmallPose.asset"
        );

        if (sphereSmallPose == null)
        {
            Debug.LogWarning("⚠️ Could not find sphereSmallPose in Assets/SteamVR/InteractionSystem/Poses/");
            return;
        }

        foreach (Rigidbody rb in allRigidbodies)
        {
            GameObject obj = rb.gameObject;

            if (obj.GetComponent<VelocityEstimator>() == null)
                obj.AddComponent<VelocityEstimator>();

            // ===================== Interactable =====================
            Interactable interactable = obj.GetComponent<Interactable>();
            if (interactable == null)
                interactable = obj.AddComponent<Interactable>();

            // ✅ Configuración de los "Hide"
            interactable.hideHandOnAttach = false;
            interactable.hideSkeletonOnAttach = false;
            interactable.hideControllerOnAttach = true;

            // ===================== Throwable =====================
            Throwable throwable = obj.GetComponent<Throwable>();
            if (throwable == null)
                throwable = obj.AddComponent<Throwable>();

            throwable.attachmentFlags =
                Hand.AttachmentFlags.SnapOnAttach |
                Hand.AttachmentFlags.DetachFromOtherHand |
                Hand.AttachmentFlags.VelocityMovement;

            // ===================== Skeleton Poser =====================
            SteamVR_Skeleton_Poser poser = obj.GetComponent<SteamVR_Skeleton_Poser>();
            if (poser == null)
                poser = obj.AddComponent<SteamVR_Skeleton_Poser>();

            poser.skeletonMainPose = sphereSmallPose;

            // 🔹 Marca como modificado
            EditorUtility.SetDirty(poser);
            EditorUtility.SetDirty(sphereSmallPose);
            EditorUtility.SetDirty(interactable);

            rb.useGravity = true;
            addedCount++;
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"✅ Configurado y guardado para {addedCount} objetos con Rigidbody.");
    }
}
