using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Material selectMaterial;
    [SerializeField]
    private GearSystem gearSystem;
    [SerializeField]
    private UIVisuals visuals;

    private PlanetarySystemElement currentSelected;
    private Gear otherSelected;
    private bool isGroupSelected = false;
    private bool lockOthersPanel = false;
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
                            currentSelected.ResetMaterial();
                            currentSelected = element;
                            SelectGroup();
                        }
                        else
                        {
                            if (isGroupSelected)
                            {
                                if (gearSystem is PlanetarySystem pis)
                                {
                                    pis.SelectPlanetGearGroup(selectMaterial, true);
                                    element.SetMaterial(selectMaterial);
                                    isGroupSelected = false;
                                    gearSystem.SetDriverRotator(element);
                                    visuals.DriverInformation(true, true, gearSystem.DriverSpeed, gearSystem.DriverTorque);
                                    visuals.SetCogsInformation(currentSelected.Cogs, 1);
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
    private void SelectSingleGear(PlanetarySystemElement element)
    {
        if (currentSelected != null)
        {
            if (currentSelected != element)
            {
                if (isGroupSelected)
                {
                    if (gearSystem is PlanetarySystem pis)
                    {
                        pis.SelectPlanetGearGroup(selectMaterial, true);
                        isGroupSelected = false;

                    }
                }
                else
                {
                    currentSelected.ResetMaterial();
                }
                element.SetMaterial(selectMaterial);
                currentSelected = element;
                gearSystem.SetDriverRotator(element);
            }
        }
        else
        {
            currentSelected = element;
            element.SetMaterial(selectMaterial);
            gearSystem.SetDriverRotator(element);
        }
        if (element.gearType == GearTypePlSystem.RingGear && gearSystem is PlanetarySystem sys)
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
        if (gearSystem is PlanetarySystem sys)
        {
            sys.RebuildSystem(currentSelected, visuals.SliderValue);
            if (isGroupSelected)
            {
                sys.SelectPlanetGearGroup(selectMaterial, false);
                visuals.SetCogsInformation(currentSelected.Cogs, 3);
            }
            else
            {
                currentSelected.SetMaterial(this.selectMaterial);
                visuals.SetCogsInformation(currentSelected.Cogs, 1);
            }
        }
    }
    private void SelectGroup()
    {
        isGroupSelected = true;
        if (gearSystem is PlanetarySystem ps)
        {
            ps.SelectPlanetGearGroup(selectMaterial, false);
            ps.SetDriverRotator(ps.GetPlanetaryAxis());
        }
        visuals.DriverInformation(true, true, gearSystem.DriverSpeed, gearSystem.DriverTorque);
        visuals.SetCogsInformation(currentSelected.Cogs,3);

    }
    private void SelectNothing()
    {
        if (currentSelected != null)
        {
            if (isGroupSelected)
            {
                if (gearSystem is PlanetarySystem ps)
                {
                    ps.SelectPlanetGearGroup(selectMaterial, true);
                }
            }
            else
                currentSelected.ResetMaterial();
        }
        visuals.DriverInformation(false, false, gearSystem.DriverSpeed, gearSystem.DriverTorque);
        currentSelected = null;
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
