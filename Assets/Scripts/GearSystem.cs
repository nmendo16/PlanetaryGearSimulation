using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearSystem : RotatingSystem
{
    private void Update()
    {
        float xAxis = Input.GetAxis("Horizontal");

        if (xAxis != previousAxis && drivingGear != null)
        {
            drivingGear.SetForFrame(driverSpeed * xAxis, driverTorque);
            PropagateGroup(drivingGear, drivingGear.Neighbors, false);
            PropagateGroup(drivingGear, drivingGear.Joints, true);
            previousAxis = xAxis;
        }
    }

}
