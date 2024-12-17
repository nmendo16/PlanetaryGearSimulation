using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(PlanetGearGenerator))]
public class Gear : RotatingElement
{
    private PlanetGearGenerator genGear;
    [SerializeField]
    private Material defaultMaterial;

    protected void Awake()
    {
        genGear = GetComponent<PlanetGearGenerator>();
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
    public void SetLayerAll(string layerName)
    {
        SetLayerAll(gameObject, layerName);
    }
    public void SetLayerAll(GameObject obj, string layerName)
    {
        int layer = -1;
        if (obj.CompareTag("CogMesh"))
        {
            layer = LayerMask.NameToLayer(layerName + "Cog");
        }
        else if (obj.CompareTag("RingMesh"))
        {
            layer = LayerMask.NameToLayer(layerName + "Ring");
        }
        else
        {
            layer = LayerMask.NameToLayer(layerName);
        }
        if (layer < 0) layer = 0;
        obj.layer = layer;
        if (obj.transform.childCount > 0)
        {
            foreach (Transform child in obj.transform)
            {
                SetLayerAll(child.gameObject, layerName);
            }
        }
    }
    public void DeleteGeneratedModel()
    {
        genGear.Destroy();
    }
}
