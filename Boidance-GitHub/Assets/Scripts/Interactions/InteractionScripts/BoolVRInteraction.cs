using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[CreateAssetMenu(menuName = "Flock/Interaction/Bool VR")]
public class BoolVRInteraction : BoidInteraction
{

    [HideInInspector]
    public string regexName = "System.Boolean";
    public bool debugValue;
    private bool value = false;
    [HideInInspector]
    public List<string> valueFields;
    [HideInInspector]
    public UnityEngine.XR.InputFeatureUsage<bool> featureValue;
    
    public BoolVRInteraction()
    {
        data.regexName = regexName;
    }

    public override Vector4 GetInputValue()
    {
        newVector = new Vector4();
        if (featureValue.name != null) // check if feature is not null
        {
            float floatValue = 0;
            var devices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(data.deviceCharacteristics, devices);
            UnityEngine.XR.InputFeatureUsage<bool> featureValue2 = CommonUsages.triggerButton;
            foreach (var device in devices)
            {
                if (device.TryGetFeatureValue(featureValue, out value))
                {
                    floatValue = value ? 1 : 0;
                    newVector += new Vector4(floatValue, 0, 0);
                }
            }
            if (devices.Count == 0)
            {
                floatValue = debugValue ? 1 : 0;
                newVector += new Vector4(floatValue, 0, 0);
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
        featureValue = (InputFeatureUsage<bool>)typeof(CommonUsages).GetField(name).GetValue(typeof(CommonUsages).GetField(name));
        data.propIndex = index;
    }
}
