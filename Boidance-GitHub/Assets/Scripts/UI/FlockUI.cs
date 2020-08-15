using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FlockUI : MonoBehaviour
{
    public Flock flock;
    public Button menuButton;
    public Slider numberSlider;
    public TMP_InputField sizeInput;
    public TMP_InputField speedInput;
    public TMP_InputField sightRadiusInput;
    public TMP_InputField avoidsRadiusInput;
    public Transform topBar;
    public ColorPicker colorPicker;

    public Toggle leftHand;
    public Toggle rightHand;
    public Toggle head;

    public Toggle matchShape;
    public Toggle matchSize;

    public Toggle matchTriggerL;
    public Toggle matchTriggerR;

    public Button optionButton;
    public static bool gameIsOption = false;
    public GameObject optionMenu;

    [SerializeField]
    public SaveSystem saveSystem;
    public void Start()
    {
        sizeInput.text = saveSystem.CurrentData.scale.ToString(CultureInfo.InvariantCulture);
        speedInput.text = saveSystem.CurrentData.maxSpeed.ToString(CultureInfo.InvariantCulture);
        sightRadiusInput.text = saveSystem.CurrentData.sightRadius.ToString(CultureInfo.InvariantCulture);
        avoidsRadiusInput.text = saveSystem.CurrentData.sqrtAvoidanceRadius.ToString(CultureInfo.InvariantCulture);
        numberSlider.value = flock.startingCount;

        menuButton.onClick.AddListener(OnMenuButtonClicked);
        optionButton.onClick.AddListener(OnOptionClicked);
        numberSlider.onValueChanged.AddListener(delegate { OnNumberSliderChanged(); });
        sizeInput.onValueChanged.AddListener(delegate { OnSizeInputChanged(); });
        speedInput.onValueChanged.AddListener(delegate { OnSpeedInputChanged(); });
        sightRadiusInput.onValueChanged.AddListener(delegate { OnRadiusInputChanged(); });
        avoidsRadiusInput.onValueChanged.AddListener(delegate {OnAvoidRadiusInputChanged(); });
        leftHand.onValueChanged.AddListener(delegate { OnFollowSpeedChange(); });
        head.onValueChanged.AddListener(delegate { OnFollowSpeedChange(); });
        rightHand.onValueChanged.AddListener(delegate { OnFollowSpeedChange(); });
        matchShape.onValueChanged.AddListener(delegate { OnMatchShapeChange(); });
        matchSize.onValueChanged.AddListener(delegate { OnMatchSizeChange(); });
        matchTriggerL.onValueChanged.AddListener(delegate { OnTriggerMatchChange(); });
        matchTriggerR.onValueChanged.AddListener(delegate { OnTriggerMatchChange(); });


        saveSystem.OnPreferencesChange += OnPreferencesChange;
        OnPreferencesChange(saveSystem.currentData);

    }

    public void OnMatchShapeChange()
    {
        saveSystem.currentData.matchShape = !saveSystem.currentData.matchShape;
        if (!saveSystem.currentData.matchShape)
        {
            flock.ChangeShapeOfAgent(null, null, true);
        }
    }
    public void OnMatchSizeChange()
    {
        saveSystem.currentData.matchSize = !saveSystem.currentData.matchSize;
        if (!saveSystem.currentData.matchSize)
        {
            flock.ChangeSizeOfAgent();
        }
    }

    private void OnPreferencesChange(PreferencesData newval)
    {
        sizeInput.text = saveSystem.currentData.scale.ToString(CultureInfo.InvariantCulture);
        speedInput.text = saveSystem.currentData.maxSpeed.ToString(CultureInfo.InvariantCulture);
        sightRadiusInput.text = saveSystem.currentData.sightRadius.ToString(CultureInfo.InvariantCulture);
        avoidsRadiusInput.text = saveSystem.currentData.sqrtAvoidanceRadius.ToString(CultureInfo.InvariantCulture);
        colorPicker.UpdateColor(saveSystem.currentData.MeanColor);
        leftHand.SetIsOnWithoutNotify(saveSystem.currentData.followLeft);
        rightHand.SetIsOnWithoutNotify(saveSystem.currentData.followRight);
        head.SetIsOnWithoutNotify(saveSystem.currentData.followHead);
        matchShape.SetIsOnWithoutNotify(saveSystem.CurrentData.matchShape);
        matchSize.SetIsOnWithoutNotify(saveSystem.CurrentData.matchSize);
        matchTriggerL.SetIsOnWithoutNotify(saveSystem.CurrentData.triggerLeft);
        matchTriggerR.SetIsOnWithoutNotify(saveSystem.CurrentData.triggerRight);
        OnSizeInputChanged();
        OnSpeedInputChanged();
        OnRadiusInputChanged();
        OnAvoidRadiusInputChanged();
    }

    private void OnTriggerMatchChange()
    {
        saveSystem.currentData.triggerLeft = matchTriggerL.isOn;
        saveSystem.currentData.triggerRight = matchTriggerR.isOn;
    }

    private void OnFollowSpeedChange()
    {
        saveSystem.currentData.followLeft = leftHand.isOn;
        saveSystem.currentData.followRight = rightHand.isOn;
        saveSystem.currentData.followHead = head.isOn;
    }

    public void OnNumberSliderChanged()
    {
        flock.startingCount = numberSlider.value;
        flock.ChangeNumberOfAgent();
    }
    public void OnSpeedInputChanged()
    {
        flock.maxSpeed = float.Parse(speedInput.text, CultureInfo.InvariantCulture);
        saveSystem.currentData.MaxSpeed = float.Parse(speedInput.text, CultureInfo.InvariantCulture);
    }
    public void OnRadiusInputChanged()
    {
        flock.neighborRadius = float.Parse(sightRadiusInput.text, CultureInfo.InvariantCulture);
        saveSystem.currentData.SightRadius = float.Parse(sightRadiusInput.text, CultureInfo.InvariantCulture);
    }

    public void OnAvoidRadiusInputChanged()
    {
        flock.SquareAvoidanceRadius = float.Parse(avoidsRadiusInput.text, CultureInfo.InvariantCulture);
        saveSystem.currentData.sqrtAvoidanceRadius = float.Parse(avoidsRadiusInput.text, CultureInfo.InvariantCulture);
    }

    public void OnSizeInputChanged()
    {
        flock.agentSize = float.Parse(sizeInput.text, CultureInfo.InvariantCulture);
        flock.ChangeSizeOfAgent();
        saveSystem.currentData.Scale = float.Parse(sizeInput.text, CultureInfo.InvariantCulture);
    }

    public void OnMenuButtonClicked()
    {
        topBar.gameObject.SetActive(!topBar.gameObject.activeSelf);
    }

    public void OnOptionClicked()
    {
        if (gameIsOption)
        {
            Resume();
        } else
        {
            Pause();
        }
    }

    void Resume()
    {
        optionMenu.SetActive(false);
        gameIsOption = false;
    }

    void Pause()
    {
        optionMenu.SetActive(true);
        gameIsOption = true;
    }
}
