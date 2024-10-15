using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIVisuals : MonoBehaviour
{
    [Header("Information panels and holders")]
    [SerializeField]
    private GameObject sliderHolder;
    [SerializeField]
    private GameObject driverPanel;
    [SerializeField]
    private GameObject othersPanel;
    [Header("Individual elements")]
    [SerializeField]
    private Slider cogsSlider;
    [SerializeField]
    private TextMeshProUGUI cogsText;


    private UIDriverPanelPS driverPanelManager;
    private UIDriverPanelPS othersPanelManager;

    public int SliderValue { get { return (int)cogsSlider.value; } }

    private void Start()
    {
        driverPanelManager = driverPanel.GetComponent<UIDriverPanelPS>();
        othersPanelManager = othersPanel.GetComponent<UIDriverPanelPS>();
        sliderHolder.SetActive(false);
        driverPanel.SetActive(false);
        othersPanel.SetActive(false);
    }
    public void SetCogsInformation(int no_cogs, int cogsSelected)
    {
        sliderHolder.SetActive(true);
        this.cogsSlider.value = no_cogs;
        this.cogsText.text = no_cogs.ToString();
        if (cogsSelected > 1)
        {
            this.cogsText.text += "(" + cogsSelected.ToString() + ")";
        }
        UpdateNonEditables(0, no_cogs);
    }
    public void OthersInformation(bool display, float torque = 0)
    {
        othersPanel.SetActive(display);
        othersPanelManager.UISpeed = "0";
        othersPanelManager.UITorque = torque.ToString();
        othersPanelManager.ActivateRingGearPanels(false);
        othersPanelManager.MaxSpeedWrapper(false);
        othersPanelManager.CogsWrapper(display);
    }
    public void DriverInformation(bool display, bool hasCogs, float maxSpeed, float torque)
    {
        sliderHolder.SetActive(hasCogs);
        driverPanel.SetActive(display);

        driverPanelManager.UIMaxSpeed = maxSpeed.ToString();
        driverPanelManager.UITorque = torque.ToString();
        driverPanelManager.ActivateRingGearPanels(false); // assume that, baseline, is not a ring gear. 
    }
    public void ActivateExtraRingGearInfo(bool isLocked)
    {
        driverPanelManager.ActivateRingGearPanels(true);
        driverPanelManager.IsRingGearLocked = isLocked;
    }
    public void UpdateNonEditables(float speed, float cogs, bool isDriver = true)
    {
        if (isDriver)
        {
            if (driverPanel.activeInHierarchy)
            {
                driverPanelManager.UISpeed = speed.ToString();
                driverPanelManager.UICogs = cogs.ToString();
            }
        }
        else
        {
            if (othersPanel.activeInHierarchy)
            {
                othersPanelManager.UISpeed = speed.ToString();
                othersPanelManager.UICogs = cogs.ToString();
            }
        }
    }
    public void UpdateTorque(float torque, bool isDriver = true)
    {
        if (isDriver)
        {
            driverPanelManager.UITorque = torque.ToString();
        }
        else
        {
            othersPanelManager.UITorque = torque.ToString();
        }
    }
    public float OnEditMaxSpeed()
    {
        driverPanelManager.UIMaxSpeed = driverPanelManager.UIMaxSpeed;
        return float.Parse(driverPanelManager.UIMaxSpeed);
    }
    public float OnEditTorque()
    {
        driverPanelManager.UITorque = driverPanelManager.UITorque;
        return float.Parse(driverPanelManager.UITorque);
    }
    public bool IsRingGearLocked()
    {
        return this.driverPanelManager.IsRingGearLocked;
    }


}
