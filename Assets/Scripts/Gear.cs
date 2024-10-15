using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GearGeneration))]
public class Gear : RotatingElement
{
    private GearGeneration genGear;
    [SerializeField]
    private Material defaultMaterial;

    protected void Awake()
    {
        genGear = GetComponent<GearGeneration>();
    }

    public float GetTotalRadius()
    {
        return (this.genGear.GetBodySize().y / 2) + this.genGear.GetTeethSize().y;
    }
    public float GetBodyRadius()
    {
        return this.genGear.GetBodySize().y / 2;
    }
    public void GenerateGear(int no_Teeth)
    {
        this.n_cogs = no_Teeth;
        genGear.Generate(no_Teeth);
    }
    public void AddNeighbor(RotatingElement neighbor)
    {
        this.neighbors.Add(neighbor);
    }
    public void RemoveNeighbor(RotatingElement notANeighhbor)
    {
        this.neighbors.Remove(notANeighhbor);
    }
    public void SetMaterial(Material material)
    {
        foreach (MeshRenderer mr in genGear.GetMeshRenderers())
        {
            mr.material = material;
        }
    }
    public void ResetMaterial()
    {
        foreach (MeshRenderer mr in genGear.GetMeshRenderers())
        {
            mr.material = defaultMaterial;
        }
    }
    public void DeleteGeneratedModel()
    {
        genGear.Destroy();
    }
}
