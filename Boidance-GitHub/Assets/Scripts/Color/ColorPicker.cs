using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ColorPicker : MonoBehaviour
{
    public Slider r;
    public Slider g;
    public Slider b;

    public Toggle fromWheelToggle;

    public Material targetMaterial;

    public SaveSystem saveSystem;
    public Flock flock;

    public float x;
    public float y;

    private void Start()
    {
        r.onValueChanged.AddListener(delegate { ChangeColor(); });
        g.onValueChanged.AddListener(delegate { ChangeColor(); });
        b.onValueChanged.AddListener(delegate { ChangeColor(); });
        if (targetMaterial)
        {
            b.value = targetMaterial.GetColor("_Tint").b;
            r.value = targetMaterial.GetColor("_Tint").r;
            g.value = targetMaterial.GetColor("_Tint").g;
        }
        else
        {
            fromWheelToggle.onValueChanged.AddListener(delegate { OnWheelCheck(); });
            r.SetValueWithoutNotify(saveSystem.CurrentData.MeanColor.r);
            g.SetValueWithoutNotify(saveSystem.CurrentData.MeanColor.g);
            b.SetValueWithoutNotify(saveSystem.CurrentData.MeanColor.b);
            fromWheelToggle.SetIsOnWithoutNotify(saveSystem.currentData.matchWheel);
        }
        ChangeColor();
    }

    private void OnWheelCheck()
    {
        saveSystem.currentData.matchWheel = fromWheelToggle.isOn;
    }

    private void ChangeColor()
    {
        if (targetMaterial)
        {
            targetMaterial.SetColor("_Tint", new Color(r.value, g.value, b.value, 1));
        }
        else if (saveSystem)
        {
            saveSystem.currentData.MeanColor = new Color(r.value, g.value, b.value, 1);
            flock.ChangeColorOfAgent(saveSystem.currentData.MeanColor);
        }
    }

    public void UpdateColor(Color color)
    {
        r.SetValueWithoutNotify(color.r);
        g.SetValueWithoutNotify(color.g);
        b.SetValueWithoutNotify(color.b);
        saveSystem.currentData.MeanColor = new Color(r.value, g.value, b.value, 1);
        flock.ChangeColorOfAgent(saveSystem.currentData.MeanColor);

    }

    public void getNewColor()
    {

        if (fromWheelToggle != null && fromWheelToggle.isOn)
        {
            Vector2 vector2Value;
            bool boolValue;
            var devices = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(UnityEngine.XR.InputDeviceCharacteristics.HeldInHand, devices);
            UnityEngine.XR.InputFeatureUsage<bool> featureValue2 = CommonUsages.primary2DAxisTouch;
            UnityEngine.XR.InputFeatureUsage<Vector2> featureValue = CommonUsages.primary2DAxis;
            foreach (var device in devices)
            {
                if (device.TryGetFeatureValue(featureValue2, out boolValue))
                {
                    if (boolValue)
                    {
                        if (device.TryGetFeatureValue(featureValue, out vector2Value))
                        {
                            if (vector2Value.y >= 0)
                            {
                                float h = Vector2.Angle(Vector2.right, vector2Value)/360;
                                float s = 1f;
                                float v = vector2Value.magnitude;
                                Color color = Color.HSVToRGB(h, s, v);
                                UpdateColor(color);
                            }
                            else
                            {
                                float h = (360 - Vector2.Angle(Vector2.right, vector2Value)) / 360;
                                float s = 1f;
                                float v = vector2Value.magnitude;
                                Color color = Color.HSVToRGB(h, s, v);
                                UpdateColor(color);
                            }

                        }
                    }
                }
            }
            //Debug purpose
/*            if (false)
            {
                float h = Vector2.Angle(Vector2.right, new Vector2(x, y)) / 360;
                UpdateColor(Color.HSVToRGB(h, 1, (new Vector2(x, y).magnitude)));
            }*/

        }
    }
}
