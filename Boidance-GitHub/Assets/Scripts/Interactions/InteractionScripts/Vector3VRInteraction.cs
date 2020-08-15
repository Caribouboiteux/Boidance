using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flock/Interaction/Vector3 VR")]
public class Vector3VRInteraction : BoidInteraction
{
    [HideInInspector]
    public string regexName = "UnityEngine.Vector3";
    public Vector3 debugValue;
    private Vector3 value = Vector3.zero;
    [HideInInspector]
    public List<string> valueFields;
    [HideInInspector]
    public UnityEngine.XR.InputFeatureUsage<Vector3> featureValue;
    public Vector3VRInteraction()
    {
        data.regexName = regexName;
    }

    public override Vector4 GetInputValue()
    {
        newVector = new Vector4();
        if (featureValue.name != null) // check if feature is not null
        {
            var devices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(data.deviceCharacteristics, devices);
            foreach (var device in devices)
            {

                if (device.TryGetFeatureValue(featureValue, out value))
                {
                    newVector += (Vector4)value;
                }
            }
            if (devices.Count == 0)
            {
                newVector += (Vector4)debugValue;
            }
            else
            {
                newVector /= devices.Count;
            }
        }
        newVector = Vector4.Scale(newVector, data.maskVector);
        VRValue = newVector;
        return newVector;
    }

    public override string getRegexName()
    {
        return regexName;
    }

    public override void SetInput(string name, int index)
    {
        featureValue = (InputFeatureUsage<Vector3>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));
        data.propIndex = index;
    }
}
