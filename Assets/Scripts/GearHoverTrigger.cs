using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearHoverTrigger : MonoBehaviour
{
    [SerializeField]
    private UIManager ui_Manager;
    [SerializeField]
    private Gear me;
    private void OnMouseEnter()
    {
        ui_Manager.OthersPanel(true, me);
    }
    private void OnMouseExit()
    {
        ui_Manager.OthersPanel(false, null);
    }
}
