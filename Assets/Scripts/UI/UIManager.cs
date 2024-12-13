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
    [SerializeField]
    private GearSystem gearSystem;
    [SerializeField]
    private UIVisuals visuals;
    [Header("Debug Information.")]
    [SerializeField]
    private PlanetarySystemElement currentSelected;
    private Gear otherSelected;
    [SerializeField]
    private bool isGroupSelected = false;
    [SerializeField]
    private bool lockOthersPanel = false;
    [SerializeField]
    private bool isHovering = false;

    // Update is called once per frame
    void Update()
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
                    if (otherSelected != null)
                    {
                        visuals.OthersInformation(false);
                        lockOthersPanel = false;
                        otherSelected = null;
                    }
                }
                else if (hit.collider.CompareTag("GearGroup"))
                {
                    if (otherSelected != null)
                    {
                        visuals.OthersInformation(false);
                        lockOthersPanel = false;
                        otherSelected = null;
                    }
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
                                if (gearSystem is PlanetarySystem ps)
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
        if (Input.GetButtonDown("RightClick"))
        {
            LockOthersPanel(!lockOthersPanel);
        }
        if (currentSelected != null)
        {
            this.visuals.UpdateNonEditables(currentSelected.Speed, currentSelected.Cogs);
        }
        if (otherSelected != null)
        {
            this.visuals.UpdateNonEditables(otherSelected.Speed, otherSelected.Cogs, isDriver: false);
            this.visuals.UpdateTorque(otherSelected.Torque, isDriver: false);
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
        // after visually selecting the gear (by adding outlines), we need to see if we need to activate special UIs.
        if (toBeSelected.gearType == GearTypePlSystem.RingGear && gearSystem is PlanetarySystem sys)
        {
            visuals.DriverInformation(true, false, gearSystem.DriverSpeed, gearSystem.DriverTorque);
            visuals.ActivateExtraRingGearInfo(sys.IsRingGearLocked);
        }
        else
        {
            visuals.DriverInformation(true, true, gearSystem.DriverSpeed, gearSystem.DriverTorque);
            visuals.SetCogsInformation(currentSelected.Cogs, 1);
        }
    }
    public void ChangeCogsInGear()
    {
        if (gearSystem is PlanetarySystem ps)
        {
            ps.RebuildSystem(currentSelected, visuals.SliderValue);
            //When changing cogs, it rebuilds the system. This means we need to re-apply if the gear was previouslt selected.
            if (isGroupSelected)
            {
                ps.VisuallySelectPlanetGroup(outlinesLayerName);
                visuals.SetCogsInformation(currentSelected.Cogs, 3);
            }
            else
            {
                currentSelected.SetLayerAll(outlinesLayerName);
                visuals.SetCogsInformation(currentSelected.Cogs, 1);
            }
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
        visuals.DriverInformation(true, true, gearSystem.DriverSpeed, gearSystem.DriverTorque);
        visuals.SetCogsInformation(currentSelected.Cogs, 3);
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
        visuals.DriverInformation(false, false, gearSystem.DriverSpeed, gearSystem.DriverTorque);
        currentSelected = null;
        isGroupSelected = false;
        gearSystem.SetDriverRotator(null);
    }
    public void ChangeMaxSpeed()
    {
        this.gearSystem.DriverSpeed = visuals.OnEditMaxSpeed();

    }
    public void ChangeTorque()
    {
        this.gearSystem.DriverTorque = visuals.OnEditTorque();
    }
    public void OnChangeRingGearLock()
    {
        if (gearSystem is PlanetarySystem ps)
        {
            ps.LockRingGear(this.visuals.IsRingGearLocked());
        }
    }
    public void OthersPanel(bool show, Gear elementSelected)
    {
        if (currentSelected != elementSelected || currentSelected == null)
        {
            isHovering = show;
            if (!lockOthersPanel)
            {
                if (isHovering)
                {
                    visuals.OthersInformation(show, elementSelected.Torque);
                    otherSelected = elementSelected;
                }
                else
                {
                    visuals.OthersInformation(show);
                    otherSelected = null;
                }
            }
        }
    }
    public void LockOthersPanel(bool lockOthers)
    {
        if (otherSelected != null)
        {
            lockOthersPanel = lockOthers;
            if (!lockOthersPanel)
            {
                if (!isHovering)
                {
                    visuals.OthersInformation(false);
                    otherSelected = null;
                }
            }
        }
    }

}
