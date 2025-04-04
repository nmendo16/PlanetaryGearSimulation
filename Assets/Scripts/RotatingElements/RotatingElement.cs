using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RotatingElement : MonoBehaviour
{
    [SerializeField]
    protected List<RotatingElement> neighbors; // -- neighbors that touch this gear. 
    [SerializeField]
    protected List<RotatingElement> joints; // -- elements that are joined with this gear. 
    [SerializeField]
    protected float torque, speed;
    [SerializeField]
    protected int n_cogs;
    [SerializeField]
    protected bool setThisFrame = false;
    public float Torque { get { return torque; } private set { torque = value; } }
    public float Speed { get { return speed; } private set { speed = value; } }
    public int Cogs { get { return n_cogs; } set { n_cogs = value; } }
    public List<RotatingElement> Neighbors { get { return neighbors; } private set { neighbors = value; } }
    public List<RotatingElement> Joints { get { return joints; } private set { joints = value; } }
    public bool SetThisFrame { get { return setThisFrame; } private set { setThisFrame = value; } }

    public bool LockRotation { get; set; }

    void OnEnable()
    {
        UIManager.StopGears += StopGear;
    }
    void OnDisable()
    {
        UIManager.StopGears += StopGear;
    }
    protected virtual void Update()
    {
        if (Speed != 0 && !LockRotation)
        {
            transform.Rotate(Speed * Time.deltaTime * transform.forward);
        }
    }
    public virtual void SetForFrame(float speed, float torque)
    {
        this.speed = speed;
        this.torque = torque;
        SetThisFrame = true;
        StartCoroutine(WaitForFrame());
        // -- wait for one frame, SetThisFrame = false;
    }

    protected virtual IEnumerator WaitForFrame()
    {
        yield return 0;
        SetThisFrame = false;
    }
    public void StopGear()
    {
        Speed = 0f;
    }
}
