using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementButtons : MonoBehaviour
{
    [Header("Settings")]
    // We need to use a speed system because we want to simulate GetAxis when moving a gear.
    [SerializeField]
    private float speed = 5f;
    [Header("Dependencies")]
    [SerializeField]
    private PlanetarySystem planetarySystem;
    private float targetValue = 0f;
    private float currentValue = 0f;

    /*private void Update()
    {
        if (targetValue == 0f) currentValue = 0f;
        else if (Mathf.Abs(currentValue) < Mathf.Abs(targetValue))
        {
            currentValue += targetValue * speed * Time.deltaTime;
        }
        Debug.Log(currentValue);
        planetarySystem.SetSystemSpeed(currentValue);
    }*/
    public void MoveGears(float targetValue)
    {
        this.targetValue = targetValue;
    }
}
