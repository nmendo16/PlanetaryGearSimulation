using System.Collections.Generic;
using UnityEngine;

public abstract class RotatingSystem : MonoBehaviour
{
    [Header("Driving Gear Settings")]
    [SerializeField]
    protected RotatingElement drivingGear;
    [SerializeField]
    protected float driverSpeed;
    [SerializeField]
    protected float driverTorque;

    public float DriverSpeed { get { return driverSpeed; } set { this.driverSpeed = value; } }
    public float DriverTorque { get { return driverTorque; } set { this.driverTorque = value; } }

    protected float previousAxis = 0;
    // In Update
    /// <summary>
    /// Calculates the movement of a Gear (Reciever) based on the Torque and Speed of an Efecting Gear (Efector).
    /// </summary>
    /// <param name="Reciever"> A Gear being moved by another. </param>
    /// <param name="Efector"> Gears that moves another</param>
    /// <param name="chain"> Stores the gears that are already moving. </param>
    /// <param name="isJoint"> Are the reciever & efector joined? </param>
    protected virtual void Propagate(RotatingElement Reciever, RotatingElement Efector, List<RotatingElement> chain, bool isJoint)
    {
        chain.Add(Reciever); // -- have this reciever moved before? In case it share neighbors.

        float speed = Efector.Speed;
        float torque = Efector.Torque;
        if (!isJoint)
        {
            float reduction = (float)Efector.Cogs / (float)Reciever.Cogs;
            reduction *= -1; // -- make it a substractor.
            speed *= reduction;
            torque /= reduction;
        }
        if (Reciever.SetThisFrame) // was set THIS frame, go here. (AKA, in this same code execution thread. )
        {
            if (SignificantlyDifferent(Reciever.Speed, speed))
            {
                Debug.Log($"{Reciever.name} blocks chain. ");
            }
        }
        else
        {
            Reciever.SetForFrame(speed, torque);
            foreach (var n in Reciever.Neighbors)
            {
                if (chain.Contains(n)) continue;
                Propagate(n, Reciever, chain, false);
            }
            foreach (var j in Reciever.Joints)
            {
                if (chain.Contains(j)) continue;
                Propagate(j, Reciever, chain, true);
            }
        }
    }
    protected bool SignificantlyDifferent(float recievedSpeed, float efectorSpeed)
    {
        return false; //what's the threshold. 
    }
    protected virtual void PropagateGroup(RotatingElement parentGear, List<RotatingElement> childs, bool areJoints)
    {
        foreach (var v in childs)
        {
            Propagate(v, parentGear, new List<RotatingElement>(), areJoints);
        }
    }
    public void SetDriverRotator(RotatingElement element)
    {
        this.drivingGear = element;
    }
    public void SetSystemSpeed(float multiplierValue)
    {
        if (multiplierValue != previousAxis && drivingGear != null)
        {
            drivingGear.SetForFrame(driverSpeed * multiplierValue, driverTorque);
            PropagateGroup(drivingGear, drivingGear.Neighbors, false);
            PropagateGroup(drivingGear, drivingGear.Joints, true);
            previousAxis = multiplierValue;
        }
    }
    public void StopSystem()
    {
        if (drivingGear != null)
        {
            drivingGear.SetForFrame(0, driverTorque);
        }
        PropagateGroup(drivingGear, drivingGear.Neighbors, false);
        PropagateGroup(drivingGear, drivingGear.Joints, true);
        previousAxis = 0;
    }
}
