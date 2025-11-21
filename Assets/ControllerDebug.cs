using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class DebugEveryButton : MonoBehaviour
{
    List<InputDevice> devices = new List<InputDevice>();
    List<InputFeatureUsage> features = new List<InputFeatureUsage>();

    void Update()
    {
        InputDevices.GetDevices(devices);

        foreach (var device in devices)
        {
            device.TryGetFeatureUsages(features);

            foreach (var feature in features)
            {
                if (feature.type == typeof(bool))
                {
                    bool v;
                    if (device.TryGetFeatureValue(feature.As<bool>(), out v) && v)
                    {
                        Debug.Log($"[{device.name}] Button PRESSED: {feature.name}");
                    }
                }
            }
        }
    }
}
