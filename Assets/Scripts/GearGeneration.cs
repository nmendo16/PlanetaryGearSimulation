using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class GearGeneration : MonoBehaviour
{
    
    [Header ("Settings")]
    [SerializeField]
    [Tooltip("Number of teeth this cog will have.")]
    [Range(4, 70)]
    protected int n_teeth;
    [Header ("Templates")]
    [Tooltip("A reference to a prefab containing the template for a cog's body. ")]
    [SerializeField]
    protected GameObject t_cogBody;
    [Tooltip("A reference to a prefab containint the template for a cog's teeth. ")]
    [SerializeField]
    protected GameObject t_cogTeeth;

    protected MeshCollider m_Collider;

    protected List<GameObject> cogTeethInScene;
    protected GameObject cogBodyInScene;

    protected MeshRenderer mr_cogBody;
    protected MeshRenderer mr_cogTeeth;

    public int Cogs { get { return n_teeth; } private set { n_teeth = value; } }


    protected virtual void Awake()
    {

        mr_cogBody = t_cogBody.GetComponentInChildren<MeshRenderer>();
        mr_cogTeeth = t_cogTeeth.GetComponentInChildren<MeshRenderer>();
        m_Collider = GetComponent<MeshCollider>();
        cogTeethInScene = new();
    }
    /// <summary>
    /// Generates a cog model according to the specified number of teeth. 
    /// </summary>
    public virtual void Generate()
    {
        float step = 360f / (float)n_teeth;
        float radius = n_teeth / 8f; // 1f -> radius of regular body. 
        if (mr_cogBody == null) // --on editor.
        {
            mr_cogBody = t_cogBody.GetComponentInChildren<MeshRenderer>();
        }
        float gearBodyRadius = (mr_cogBody.bounds.size / 2).y; // -- diameter / 2, get its y value. 

        cogBodyInScene = Instantiate(t_cogBody, transform);
        cogBodyInScene.transform.localScale = new Vector3(radius * 1.2f * gearBodyRadius, radius * 1.2f * gearBodyRadius, 1);
        if (cogTeethInScene == null)
        {
            cogTeethInScene = new();
        }
        else
        {
            cogTeethInScene.Clear();
        }
        for ( int i = 0; i < n_teeth; i++)
        {
            cogTeethInScene.Add( Instantiate(t_cogTeeth, transform));
            cogTeethInScene[i].transform.rotation = Quaternion.Euler(0, 0, step * i);
            cogTeethInScene[i].transform.position += cogTeethInScene[i].transform.up * radius;
        }
       
    }
    public virtual void Generate (int no_teeth)
    {
        this.n_teeth = no_teeth;
        Generate();
    }
    public virtual void Destroy()
    {
        foreach (GameObject v in cogTeethInScene)
        {
            Destroy(v);
        }
        cogTeethInScene.Clear();
        Destroy(cogBodyInScene);

    }
    public Vector3 GetBodySize()
    {
        if (cogBodyInScene != null)
        {
            Debug.Log(Vector3.Scale(mr_cogBody.bounds.size, cogBodyInScene.transform.localScale));
            return Vector3.Scale(mr_cogBody.bounds.size, cogBodyInScene.transform.localScale);
        }
        return new(0, 0, 0);
    }
    public Vector3 GetTeethSize()
    {
        if (cogTeethInScene != null)
        {
            if (cogTeethInScene.Count > 0)
            {
                return Vector3.Scale(mr_cogTeeth.bounds.size, cogTeethInScene[0].transform.localScale);
            }
        }
        return new(0, 0, 0);

    }
    public List<MeshRenderer> GetMeshRenderers()
    {
        List<MeshRenderer> mr_List = new();
        mr_List.Add(cogBodyInScene.GetComponentInChildren<MeshRenderer>());
        foreach (GameObject go in cogTeethInScene)
        {
            mr_List.Add(go.GetComponentInChildren<MeshRenderer>());
        }
        return mr_List;
    }
}
