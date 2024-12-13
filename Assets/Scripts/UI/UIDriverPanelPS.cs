using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIDriverPanelPS : MonoBehaviour
{
    [Header("Editable Text References")]
    [SerializeField]
    private TMP_InputField maxSpeed;
    [SerializeField]
    private TMP_InputField torque;
    [Header("Non-editable Text References")]
    [SerializeField]
    private TextMeshProUGUI currentSpeed;
    [SerializeField]
    private TextMeshProUGUI driverCogs;
    [Header("Wrappers")]
    [SerializeField]
    private GameObject driverCogsWrapped;
    [SerializeField]
    private GameObject ringGearLockOption;
    [SerializeField]
    private GameObject maxSpeedWrapper;
    [SerializeField]
    private GameObject torqueWrapper;
    [Header("Other References")]
    [SerializeField]
    private Toggle ringGearLock;
    [Header("Settings")]
    [SerializeField]
    private float maximimPossibleSpeed = 999f;


    public string UISpeed
    {
        set
        {
            currentSpeed.text = value;
        }
    }
    public string UICogs
    {
        set
        {
            if (driverCogsWrapped.activeInHierarchy)
            {
                driverCogs.text = value;
            }
        }
    }
    public string UIMaxSpeed
    {
        get
        {
            return this.maxSpeed.text;
        }
        set
        {
            if (float.TryParse(value, out float speed))
            {
                if (maximimPossibleSpeed >= speed)
                {
                    maxSpeed.text = value;
                }
                else
                {
                    maxSpeed.text = "0";
                }
            }
            else
            {
                maxSpeed.text = "0";
            }
        }
    }
    public string UITorque
    {
        get
        {
            return this.torque.text;
        }
        set
        {
            if (float.TryParse(value, out _))
            {
                torque.text = value;
            }
            else
            {
                torque.text = "0";
            }
        }
    }
    public bool IsRingGearLocked
    {
        get
        {
            if (ringGearLockOption.activeInHierarchy)
            {
                return ringGearLock.isOn;
            }
            return false;
        }
        set
        {
            this.ringGearLock.isOn = value;
        }
    }
    public void ActivateRingGearPanels(bool activate)
    {
        ringGearLockOption.SetActive(activate);
        driverCogsWrapped.SetActive(activate);
    }
    public void MaxSpeedWrapper(bool show)
    {
        this.maxSpeedWrapper.SetActive(show);
    }
    public void TorqueWrapper(bool show)
    {
        this.torqueWrapper.SetActive(show);
    }
    public void CogsWrapper(bool show)
    {
        this.driverCogsWrapped.SetActive(show);
    }
}
