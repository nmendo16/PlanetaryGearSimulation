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
    private bool isInputBeingUsed = false;
    private bool isPlaying = false;
    public static event System.Action<bool> IsMovedByButtonEvent;

    void OnEnable()
    {
        PlanetarySystem.IsMovedByInputEvent += InputBeingUsed;
        UIManager.StopGears += StopGears;
    }
    void OnDisable()
    {
        PlanetarySystem.IsMovedByInputEvent -= InputBeingUsed;
        UIManager.StopGears -= StopGears;
    }
    private void Update()
    {
        if (!isInputBeingUsed)
        {
            if (targetValue == 0f) currentValue = 0f;
            else if (Mathf.Abs(currentValue) < Mathf.Abs(targetValue))
            {
                currentValue += targetValue * speed * Time.deltaTime;
            }
            Debug.Log(currentValue);
            planetarySystem.SetSystemSpeed(currentValue, true);
        }
    }
    public void MoveGears(float targetValue)
    {
        this.targetValue = targetValue;
        IsMovedByButtonEvent?.Invoke(targetValue != 0f);
    }
    public void PlayGears(float targetValue)
    {
        isPlaying = !isPlaying;
        this.targetValue = isPlaying ? targetValue : 0f;
        IsMovedByButtonEvent?.Invoke(targetValue != 0f);
    }
    public void StopGears()
    {
        targetValue = 0f;
        IsMovedByButtonEvent?.Invoke(false);
    }
    public void InputBeingUsed(bool isInputBeingUsed)
    {
        this.isInputBeingUsed = isInputBeingUsed;
    }
}
