using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "Flock/Interaction/Float VR")]
public class FloatVRInteraction : BoidInteraction
{
    [HideInInspector]
    public string regexName = "System.Single";
    public float debugValue;
    private float value = 0f;
    [HideInInspector]
    public List<string> valueFields;
    [HideInInspector]
    public UnityEngine.XR.InputFeatureUsage<float> featureValue;


    public FloatVRInteraction()
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
                    newVector += new Vector4(value, 0, 0);
                }
            }
            if (devices.Count == 0)
            {
                newVector += new Vector4(debugValue, 0, 0);
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
        featureValue = (InputFeatureUsage<float>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));
        data.propIndex = index;
    }
}
