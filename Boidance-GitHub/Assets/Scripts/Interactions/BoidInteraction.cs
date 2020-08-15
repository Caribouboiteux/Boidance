using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


[System.Serializable]
public abstract class BoidInteraction : ScriptableObject
{
    [System.Serializable]
    public class Data
    {
        public UnityEngine.XR.InputDeviceCharacteristics deviceCharacteristics;
        public Vector4 maskVector = Vector4.one;
        public string regexName;
        public int propIndex;
        public string interactionName = "Default";
    }
    protected Vector4 newVector;
    protected Vector4 vRValue;
    //[HideInInspector]
    //public string regexName;
    public Data data =  new Data();
    public Vector4 VRValue
    {
        get { return vRValue; }
        set
        {
            if (vRValue == value) return;
            vRValue = value;
            OnVariableChange?.Invoke(vRValue);
        }
    }

    public delegate void OnVariableChangeDelegate(Vector4 newVal);
    public event OnVariableChangeDelegate OnVariableChange;
    public abstract string getRegexName();
    public abstract void SetInput(string name, int index);
    public abstract Vector4 GetInputValue();
}
