using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
/***
Generates a basic gear taking a Prefab Gear and Cog as templates.
Can be used from the editor or at runtime. 
***/
public class PlanetGearGenerator : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField]
    [Tooltip("Number of teeth this gear will have.")]
    [Range(4, 70)]
    protected int n_cogs;
    [Header("Templates")]
    [Tooltip("A reference to a prefab containing the template for a gear's body. ")]
    [SerializeField]
    protected GameObject obj_gear;
    [Tooltip("A reference to a prefab containint the template for a cog ")]
    [SerializeField]
    protected GameObject obj_cog;

    protected List<GameObject> l_cogsInScene;
    protected GameObject gearInScene;

    protected MeshRenderer mesh_gear;
    protected MeshRenderer mesh_cog;

    public int Cogs { get { return n_cogs; } private set { n_cogs = value; } }


    protected virtual void Awake()
    {
        mesh_gear = obj_gear.GetComponentInChildren<MeshRenderer>();
        mesh_cog = obj_cog.GetComponentInChildren<MeshRenderer>();
        l_cogsInScene = new();
    }
    /// <summary>
    /// Generates a cog model according to the specified number of teeth. 
    /// </summary>
    public virtual void Generate()
    {
        float step = 360f / (float)n_cogs;
        float radius = n_cogs / 8f; // -- A cog wiht a radius of 1f is approximately 8 cogs in radius. 
        if (mesh_gear == null) // -- This check is needed in case the gear is generated on editor. 
        {
            mesh_gear = obj_gear.GetComponentInChildren<MeshRenderer>();
        }
        float gearBodyRadius = (mesh_gear.bounds.size / 2).y; // mesh_gear.bounds.size is pretty much the diameter of the gear's body.

        gearInScene = Instantiate(obj_gear, transform);
        gearInScene.transform.localScale = new Vector3(radius * 1.2f * gearBodyRadius, radius * 1.2f * gearBodyRadius, 1);
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
            l_cogsInScene[i].transform.position += l_cogsInScene[i].transform.up * radius;
        }

    }
    public virtual void Generate(int no_teeth)
    {
        this.n_cogs = no_teeth;
        Generate();
    }
    public virtual void Destroy()
    {
        foreach (GameObject v in l_cogsInScene)
        {
            Destroy(v);
        }
        l_cogsInScene.Clear();
        Destroy(gearInScene);

    }
    public Vector3 GetBodySize()
    {
        if (gearInScene != null)
        {
            Debug.Log(Vector3.Scale(mesh_gear.bounds.size, gearInScene.transform.localScale));
            return Vector3.Scale(mesh_gear.bounds.size, gearInScene.transform.localScale);
        }
        return new(0, 0, 0);
    }
    public Vector3 GetTeethSize()
    {
        if (l_cogsInScene != null)
        {
            if (l_cogsInScene.Count > 0)
            {
                return Vector3.Scale(mesh_cog.bounds.size, l_cogsInScene[0].transform.localScale);
            }
        }
        return new(0, 0, 0);

    }
    public List<MeshRenderer> GetMeshRenderers()
    {
        List<MeshRenderer> mr_List = new();
        mr_List.Add(gearInScene.GetComponentInChildren<MeshRenderer>());
        foreach (GameObject go in l_cogsInScene)
        {
            mr_List.Add(go.GetComponentInChildren<MeshRenderer>());
        }
        return mr_List;
    }
}
