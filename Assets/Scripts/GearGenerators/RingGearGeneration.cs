using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class RingGearGenerator : GearGenerator
{
    [Header("Ring Gear Specifics")]
    [SerializeField]
    private float outerRingRatio = 0.3f;

    private float teethSize = 0f;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        teethSize = this.obj_cog.GetComponentInChildren<MeshRenderer>().bounds.size.y; // size of teeth in z. 
    }

    public override void Generate()
    {
        float step = 360f / (float)n_cogs;
        float radius = n_cogs / 8f; // 1f -> radius of regular body. 

        if (mesh_gear == null) // --on editor.
        {
            mesh_gear = obj_gear.GetComponentInChildren<MeshRenderer>();
        }
        float gearBodyRadius = (mesh_gear.bounds.size / 2).y; // -- diameter / 2, get its y value. 
        Debug.Log(gearBodyRadius);

        gearInScene = Instantiate(obj_gear, transform);
        Vector3 cogBodyScale = new(radius * 1.2f, radius * 1.18f, 1);
        float newRadius = cogBodyScale.z * outerRingRatio / 2;
        cogBodyScale.x += newRadius + teethSize;
        cogBodyScale.y += newRadius + teethSize;

        gearInScene.transform.localScale = cogBodyScale;

        if (l_cogsInScene == null)
        {
            l_cogsInScene = new();
        }
        else
        {
            l_cogsInScene.Clear();
        }
        for (int i = 0; i < n_cogs; i++)
        {
            l_cogsInScene.Add(Instantiate(obj_cog, transform));
            l_cogsInScene[i].transform.rotation = Quaternion.Euler(0, 0, step * i);
            l_cogsInScene[i].transform.position += l_cogsInScene[i].transform.up * (radius + mesh_cog.bounds.size.y);
        }

    }
}
