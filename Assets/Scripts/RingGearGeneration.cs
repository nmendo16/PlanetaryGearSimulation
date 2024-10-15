using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RingGearGeneration : GearGeneration
{
    [Header("Ring Gear Specifics")]
    [SerializeField]
    private float outerRingRatio = 0.3f;

    private float teethSize = 0f;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        teethSize = this.t_cogTeeth.GetComponentInChildren<MeshRenderer>().bounds.size.y; // size of teeth in z. 
    }

    public override void Generate()
    {
        float step = 360f / (float)n_teeth;
        float radius = n_teeth / 8f; // 1f -> radius of regular body. 

        if (mr_cogBody == null) // --on editor.
        {
            mr_cogBody = t_cogBody.GetComponentInChildren<MeshRenderer>();
        }
        float gearBodyRadius = (mr_cogBody.bounds.size / 2).y; // -- diameter / 2, get its y value. 
        Debug.Log(gearBodyRadius);

        cogBodyInScene = Instantiate(t_cogBody, transform);
        Vector3 cogBodyScale = new (radius * 1.15f, radius * 1.15f, 1);
        float newRadius = (cogBodyScale.z * outerRingRatio) / 2;
        cogBodyScale.x += newRadius + teethSize;
        cogBodyScale.y += newRadius + teethSize;

        cogBodyInScene.transform.localScale = cogBodyScale;

        if (cogTeethInScene == null)
        {
            cogTeethInScene = new();
        }
        else
        {
            cogTeethInScene.Clear();
        }
        for (int i = 0; i < n_teeth; i++)
        {
            cogTeethInScene.Add(Instantiate(t_cogTeeth, transform));
            cogTeethInScene[i].transform.rotation = Quaternion.Euler(0, 0, step * i);
            cogTeethInScene[i].transform.position += cogTeethInScene[i].transform.up * radius;
        }

    }
}
