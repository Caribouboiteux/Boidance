using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "Flock/Interaction/Quaternion VR")]
public class QuaternionVRInteraction : BoidInteraction
{
    [HideInInspector]
    public string regexName = "UnityEngine.Quaternion";
    public Quaternion debugValue;
    private Quaternion value;
    [HideInInspector]
    public List<string> valueFields;
    [HideInInspector]
    public UnityEngine.XR.InputFeatureUsage<Quaternion> featureValue;


    public QuaternionVRInteraction()
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
                    newVector += new Vector4(value.w, value.x, value.y, value.z);
                }
            }
            if (devices.Count == 0)
            {
                newVector += new Vector4(debugValue.w, debugValue.x, debugValue.y, debugValue.z);
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
        featureValue = (InputFeatureUsage<Quaternion>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));
        data.propIndex = index;
    }
}
