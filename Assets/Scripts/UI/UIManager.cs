using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private string outlinesLayerName = "Outlined";
    [Tooltip("Name of the Layer which gives an outline to a 3d object. ")]
    [SerializeField]
    private string defaultLayerName = "Default";
    [Tooltip("Name of the default layer that all objects use. ")]
    private GearSystem gearSystem;
    [SerializeField]
    private UIVisuals visuals;
    [Header("Debug Information.")]
    [SerializeField]
    private PlanetarySystemElement currentSelected;
    [SerializeField]
    private bool isGroupSelected = false;

    private List<PlanetarySystemElement> gearsInSystem;

    void Update()
    {
        if (gearSystem != null)
        {
            if (Input.GetButtonDown("Click"))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.collider.CompareTag("Gear"))
                    {
                        PlanetarySystemElement element = hit.collider.GetComponentInParent<PlanetarySystemElement>();
                        SelectSingleGear(element);
                    }
                    else if (hit.collider.CompareTag("GearGroup"))
                    {
                        PlanetarySystemElement element = hit.collider.GetComponentInParent<PlanetarySystemElement>();
                        if (currentSelected != null)
                        {
                            if (currentSelected != element)
                            {
                                currentSelected.SetLayerAll(defaultLayerName);
                                currentSelected = element;
                                SelectGroup();
                            }
                            else
                            {
                                if (isGroupSelected)
                                {
                                    if (gearSystem is PlanetarySystem planetSystem)
                                    {
                                        SelectSingleGear(element);
                                    }
                                }
                            }
                        }
                        else
                        {
                            currentSelected = element;
                            SelectGroup();
                        }
                    }
                    else
                    {
                        SelectNothing();
                    }
                }
                else
                {
                    SelectNothing();
                }
            }
            // Every frame we have to update the current speed of each gear here:

            if (gearSystem is PlanetarySystem ps)
            {
                if (ps.isMoving)
                    visuals.UpdateCurrentElements(ps.GearsReference);
            }
        }

    }
    public void InitializeVisuals(GearSystem gearSystem)
    {
        this.gearSystem = gearSystem;
        if (gearSystem is PlanetarySystem ps)
        {
            visuals.IsRingLocked = ps.IsRingGearLocked;
            visuals.UpdatedSunSpeed = ps.DriverSpeed;
            visuals.UpdatedPlanetSpeed = ps.DriverSpeed;
            visuals.UpdatedRingSpeed = ps.DriverSpeed;
            // Updated Cog Bars to display default values:
            visuals.SunCogDisplay = ps.SunGear.Cogs;
            visuals.SunSliderValue = ps.SunGear.Cogs;
            visuals.PlanetCogDisplay = ps.PlanetGear[0].Cogs;
            visuals.PlanetSliderValue = ps.PlanetGear[0].Cogs;
            visuals.RingCogDisplay = ps.RingGear.Cogs;
        }
    }
    private void SelectSingleGear(PlanetarySystemElement toBeSelected)
    {
        if (currentSelected != null)
        {
            // we need to do this check because we don­'t want two gears to be selected at a time, so we deselect the previously selected gear. 
            if (currentSelected != toBeSelected || isGroupSelected)
            {
                gearSystem.StopSystem(); // -- its better to reset the speed everytime there's a switch in gears.
                if (isGroupSelected)
                {
                    if (gearSystem is PlanetarySystem ps)
                    {
                        ps.VisuallySelectPlanetGroup(defaultLayerName);
                        isGroupSelected = false;
                    }
                }
                else
                {
                    currentSelected.SetLayerAll(defaultLayerName);
                }
                toBeSelected.SetLayerAll(outlinesLayerName);
                currentSelected = toBeSelected;
                gearSystem.SetDriverRotator(toBeSelected);
            }
        }
        else
        {
            currentSelected = toBeSelected;
            toBeSelected.SetLayerAll(outlinesLayerName);
            gearSystem.SetDriverRotator(toBeSelected);
        }
    }
    public void ChangeSunCogs()
    {
        if (gearSystem is PlanetarySystem ps)
        {
            ps.RebuildSystem(ps.SunGear, visuals.SunSliderValue);
            if (currentSelected != null)
            {
                if (currentSelected.gearType == GearTypePlSystem.SunGear)
                    currentSelected.SetLayerAll(outlinesLayerName);
            }
            visuals.SunCogDisplay = ps.SunGear.Cogs;
            visuals.RingCogDisplay = ps.RingGear.Cogs;
        }
    }
    public void ChangePlanetCogs()
    {
        if (gearSystem is PlanetarySystem ps)
        {
            ps.RebuildSystem(ps.PlanetGear[0], visuals.PlanetSliderValue);
            if (currentSelected != null)
            {
                if (currentSelected.gearType == GearTypePlSystem.PlanetaryGear ||
                currentSelected.gearType == GearTypePlSystem.PlanetaryAxis)
                {
                    if (isGroupSelected)
                    {
                        ps.VisuallySelectPlanetGroup(outlinesLayerName);
                    }
                    else
                    {
                        currentSelected.SetLayerAll(outlinesLayerName);
                    }
                }
            }
            visuals.PlanetCogDisplay = ps.PlanetGear[0].Cogs; //TODO: Needs to change. Info should tell if its individual or all gears. 
            visuals.RingCogDisplay = ps.RingGear.Cogs;
        }
    }
    private void SelectGroup()
    {
        isGroupSelected = true;
        if (gearSystem is PlanetarySystem ps)
        {
            ps.VisuallySelectPlanetGroup(outlinesLayerName);
            ps.SetDriverRotator(ps.GetPlanetaryAxis());
        }
        gearSystem.StopSystem(); // -- its better to reset the speed everytime there's a switch in gears.
    }
    private void SelectNothing()
    {
        if (currentSelected != null)
        {
            if (isGroupSelected)
            {
                if (gearSystem is PlanetarySystem ps)
                {
                    ps.VisuallySelectPlanetGroup(defaultLayerName);
                }
            }
            else
                currentSelected.SetLayerAll(defaultLayerName);
            gearSystem.StopSystem(); // -- its better to reset the speed everytime there's a switch in gears.
        }
        currentSelected = null;
        isGroupSelected = false;
        gearSystem.SetDriverRotator(null);
    }
    public void ChangeDriverSpeed()
    {
        // This is needed as we don't want to change the system's speed from a gear we haven't selected.
        if (currentSelected != null)
        {
            switch (currentSelected.gearType)
            {
                case GearTypePlSystem.SunGear:
                    this.gearSystem.DriverSpeed = visuals.UpdatedSunSpeed;
                    break;
                case GearTypePlSystem.PlanetaryGear:
                case GearTypePlSystem.PlanetaryAxis:
                    this.gearSystem.DriverSpeed = visuals.UpdatedPlanetSpeed;
                    break;
                case GearTypePlSystem.RingGear:
                    {
                        if (gearSystem is PlanetarySystem ps)
                        {
                            if (!ps.IsRingGearLocked)
                                this.gearSystem.DriverSpeed = visuals.UpdatedRingSpeed;
                            else
                                this.gearSystem.DriverSpeed = 0;
                        }
                        break;
                    }
            }
        }
    }
    public void ChangeTorque()
    {
        if (currentSelected != null)
        {
            switch (currentSelected.gearType)
            {
                case GearTypePlSystem.SunGear:
                    this.gearSystem.DriverTorque = visuals.UpdatedSunTorque;
                    break;
                case GearTypePlSystem.PlanetaryGear:
                case GearTypePlSystem.PlanetaryAxis:
                    this.gearSystem.DriverTorque = visuals.UpdatedPlanetTorque;
                    break;
                case GearTypePlSystem.RingGear:
                    this.gearSystem.DriverTorque = visuals.UpdatedRingTorque;
                    break;
            }
        }
    }
    public void OnChangeRingGearLock()
    {
        if (gearSystem is PlanetarySystem ps)
        {
            ps.LockRingGear(this.visuals.IsRingLocked);
        }
    }
}
