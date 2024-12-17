using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GearboxPanel : MonoBehaviour
{
    [Header("Sun Gear")]
    [SerializeField]
    private TMP_InputField sunGearSpeedInput;
    [SerializeField]
    private TMP_InputField sunGearTorqueInput;
    [SerializeField]
    private TMP_InputField sunGearCurrentSpeed;
    [SerializeField]
    private TextMeshProUGUI sunGearCogs;
    [Header("Planet Gears")]
    [SerializeField]
    private TMP_InputField planetGearsSpeedInput;
    [SerializeField]
    private TMP_InputField planetGearsTorqueInput;
    [SerializeField]
    private TMP_InputField planetGearsCurrentSpeed;
    [SerializeField]
    private TextMeshProUGUI planetGearsCogs;
    [Header("Ring Gear")]
    [SerializeField]
    private TMP_InputField ringGearSpeedInput;
    [SerializeField]
    private TMP_InputField ringGearTorqueInput;
    [SerializeField]
    private TMP_InputField ringGearCurrentSpeed;
    [SerializeField]
    private TMP_InputField ringGearCogs;
    [SerializeField]
    private Toggle lockToggle;
    [Header("Settings")]
    [SerializeField]
    private float maximimPossibleSpeed = 999f;

    private void Awake()
    {
        // -- Current speed shouldn't be able to be changed by the user; the input box is for the visuals only. 
        sunGearCurrentSpeed.interactable = false;
        planetGearsCurrentSpeed.interactable = false;
        ringGearCurrentSpeed.interactable = false;
        ringGearCogs.interactable = false;
    }
    /// <summary>
    /// Attributes that interact directly with the User Input and refreshes the UI values when any change is registered.
    /// </summary>
    public string SunSpeed
    {
        get
        {
            return this.sunGearSpeedInput.text;
        }
        set
        {
            sunGearSpeedInput.text = ParseSpeed(value);
        }
    }
    public string PlanetSpeed
    {
        get
        {
            return this.planetGearsSpeedInput.text;
        }
        set
        {
            planetGearsSpeedInput.text = ParseSpeed(value);
        }
    }
    public string RingSpeed
    {
        get
        {
            return this.ringGearSpeedInput.text;
        }
        set
        {
            ringGearSpeedInput.text = ParseSpeed(value);
        }
    }
    public string SunTorque
    {
        get
        {
            return this.sunGearTorqueInput.text;
        }
        set
        {
            sunGearTorqueInput.text = ParseTorque(value);
        }
    }
    public string PlanetTorque
    {
        get
        {
            return this.planetGearsTorqueInput.text;
        }
        set
        {
            planetGearsTorqueInput.text = ParseTorque(value);
        }
    }
    public string RingTorque
    {
        get
        {
            return this.ringGearTorqueInput.text;
        }
        set
        {
            ringGearTorqueInput.text = ParseTorque(value);
        }
    }
    public string SunCogs
    {
        set
        {
            sunGearCogs.text = value;
        }
    }
    public string PlanetCogs
    {
        set
        {
            planetGearsCogs.text = value;
        }
    }
    public string RingCogs
    {
        set
        {
            ringGearCogs.text = value;
        }
    }
    public bool LockRingGear
    {
        get
        {
            return lockToggle.isOn;
        }
        set
        {
            this.lockToggle.isOn = value;
        }
    }
    public void SetCurrentElements(string speed, string torque, GearTypePlSystem gearType)
    {
        switch (gearType)
        {
            case GearTypePlSystem.SunGear:
                this.sunGearCurrentSpeed.text = speed;
                this.sunGearTorqueInput.text = torque;
                break;
            case GearTypePlSystem.PlanetaryAxis:
            case GearTypePlSystem.PlanetaryGear:
                this.planetGearsCurrentSpeed.text = speed;
                this.planetGearsTorqueInput.text = torque;
                break;
            case GearTypePlSystem.RingGear:
                this.ringGearCurrentSpeed.text = speed;
                this.ringGearTorqueInput.text = torque;
                break;
        }
    }
    private string ParseSpeed(string value)
    {
        if (float.TryParse(value, out float speed))
        {
            if (maximimPossibleSpeed >= speed)
            {
                return value;
            }
            else
            {
                return "0";
            }
        }
        else
        {
            Debug.Log("Invalid Value");
            return "0";
        }
    }
    private string ParseTorque(string value)
    {
        if (float.TryParse(value, out _))
        {
            return value;
        }
        else
        {
            return "0";
        }
    }



}
