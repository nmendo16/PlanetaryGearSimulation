using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// THIS is a necessary class as, for some reason, Unity doesn't let you serialize enumerators (ej. GearTypePLSystem)
/// for events (like OnClick()). This is a way to skip that hoop, and reference this class instead. 
/// 
/// Must be put as a component for any input value that relates to a type of gear. (ej. SunGearSpeedInput). 
/// </summary>
public class InputGearType : MonoBehaviour
{
    [SerializeField]
    private GearTypePlSystem gearType;

    public GearTypePlSystem GearType { get { return gearType; } }
}
