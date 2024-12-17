/*
A gear system is a user-controlled rotating system which used its driver gear as the center of propagation according to a
normalized user input (0 - 1).
*/
using UnityEngine;
public class GearSystem : RotatingSystem
{
    protected virtual void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");
        SetSystemSpeed(xAxis);
    }
}
