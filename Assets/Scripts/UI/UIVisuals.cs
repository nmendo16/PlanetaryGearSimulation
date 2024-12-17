using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIVisuals : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField]
    private Slider sunSlider;
    [SerializeField]
    private Slider planetSlider;
    [Header("Gearbox Panel Reference")]
    [SerializeField]
    private GearboxPanel gearBoxManager;

    public int SunSliderValue { set { sunSlider.value = value; } get { return (int)sunSlider.value; } }
    public int PlanetSliderValue { set { planetSlider.value = value; } get { return (int)planetSlider.value; } }
    public int SunCogDisplay { set { gearBoxManager.SunCogs = value.ToString(); } }
    public int PlanetCogDisplay { set { gearBoxManager.PlanetCogs = value.ToString(); } }
    public int RingCogDisplay { set { gearBoxManager.RingCogs = value.ToString(); } }
    public float UpdatedSunSpeed
    {
        set
        {
            gearBoxManager.SunSpeed = value.ToString();
        }
        get
        {
            return ParseStringToFloat(gearBoxManager.SunSpeed);
        }
    }
    public float UpdatedPlanetSpeed
    {
        set
        {
            gearBoxManager.PlanetSpeed = value.ToString();
        }
        get
        {
            return ParseStringToFloat(gearBoxManager.PlanetSpeed);
        }
    }
    public float UpdatedRingSpeed
    {
        set
        {
            gearBoxManager.RingSpeed = value.ToString();
        }
        get
        {
            return ParseStringToFloat(gearBoxManager.RingSpeed);
        }
    }
    public float UpdatedSunTorque
    {
        set
        {
            gearBoxManager.SunTorque = value.ToString();
        }
        get
        {
            return ParseStringToFloat(gearBoxManager.SunTorque);
        }
    }
    public float UpdatedPlanetTorque
    {
        set
        {
            gearBoxManager.PlanetTorque = value.ToString();
        }
        get
        {
            return ParseStringToFloat(gearBoxManager.PlanetTorque);
        }
    }
    public float UpdatedRingTorque
    {
        set
        {
            gearBoxManager.RingTorque = value.ToString();
        }
        get
        {
            return ParseStringToFloat(gearBoxManager.RingTorque);
        }
    }
    public bool IsRingLocked
    {
        set
        {
            gearBoxManager.LockRingGear = value;
        }
        get
        {
            return gearBoxManager.LockRingGear;
        }
    }
    private float ParseStringToFloat(string value)
    {
        if (float.TryParse(value, out float parsedFloat))
        {
            return parsedFloat;
        }
        return 0f;
    }
    public void UpdateCurrentElements(List<PlanetarySystemElement> elementLists)
    {
        foreach (PlanetarySystemElement gear in elementLists)
        {
            string arrangedTorque = string.Format("{0:#0.0}", gear.Torque);

            if (gear.gearType == GearTypePlSystem.RingGear)
            {
                if (this.IsRingLocked)
                {

                    gearBoxManager.SetCurrentElements("0", arrangedTorque, gear.gearType);
                    return;
                }
            }
            string arrangedSpeed = string.Format("{0:#0.0}", gear.Speed);
            gearBoxManager.SetCurrentElements(arrangedSpeed, arrangedTorque, gear.gearType);
        }
    }
    public void ParseSunSpeed(string sentence)
    {

    }
    public void ParsePlanetSpeed(string sentence)
    {

    }
    public void ParseRingSpeed(string sentence)
    {

    }
    public void ParseSunTorque(string sentence)
    {

    }
    public void ParsePlanetTorque(string sentence)
    {

    }
    public void ParseRingTorque(string sentence)
    {

    }

}
