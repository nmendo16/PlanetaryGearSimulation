using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlanetarySystem : GearSystem
{
    [Header("Planetary Gear References")]
    [SerializeField]
    private PlanetarySystemElement sunGear;
    [SerializeField]
    private List<PlanetarySystemElement> planetaryGears;
    [SerializeField]
    private RotatingElement planetaryAxis;
    [SerializeField]
    private PlanetarySystemElement ringGear;


    [Header("Settings")]
    [SerializeField]
    [Range(4, 40)]
    private int sunGearCogs;
    [SerializeField]
    [Range(4, 40)]
    private int planetGearCogs;
    [SerializeField]
    private bool isRingGearLocked = false;

    public bool IsRingGearLocked { get { return isRingGearLocked; } }


    private void Start()
    {
        GenerateSystem();
        LockRingGear(isRingGearLocked);
    }
    public void GenerateSystem()
    {
        sunGear.GenerateGear(sunGearCogs);
        foreach (Gear gear in planetaryGears)
        {
            gear.GenerateGear(planetGearCogs);
        }
        ringGear.GenerateGear(sunGearCogs + (planetGearCogs * 2));
        planetaryAxis.Cogs = ringGear.Cogs;

        sunGear.ResetMaterial();
        ringGear.ResetMaterial();
        foreach (Gear gear in planetaryGears)
        {
            gear.ResetMaterial();
        }

        RefreshGearPositions();
    }
    public void RefreshGearPositions()
    {
        float combinedRadius = (sunGear.GetTotalRadius() + planetaryGears[0].GetBodyRadius());
        float hypothenuseMagnitude = Mathf.Sqrt(2 * (combinedRadius * combinedRadius)) / 2;
        Vector3 firstQuadrantGear = new(0, combinedRadius, 0);
        Vector3 thirdQuadrantGear = new(-hypothenuseMagnitude, -hypothenuseMagnitude, 0);
        Vector3 fourthQuadrantGear = new(hypothenuseMagnitude, -hypothenuseMagnitude, 0);
        planetaryGears[0].transform.localPosition = firstQuadrantGear;
        planetaryGears[1].transform.localPosition = thirdQuadrantGear;
        planetaryGears[2].transform.localPosition = fourthQuadrantGear;
    }
    public void LockRingGear(bool lockRingGear)
    {
        if (lockRingGear)
        {
            isRingGearLocked = true;
            this.ringGear.LockRotation = true;
            this.planetaryGears[0].AddNeighbor(planetaryAxis);
            this.ringGear.RemoveNeighbor(this.planetaryGears[0]);
        }
        else
        {
            if (isRingGearLocked == true)
            {
                isRingGearLocked = false;
                this.ringGear.LockRotation = false;
                this.planetaryGears[0].RemoveNeighbor(planetaryAxis);
                this.ringGear.AddNeighbor(this.planetaryGears[0]);
            }
        }
    }
    public void SelectPlanetGearGroup(Material selectMaterial, bool deselect)
    {
        foreach (Gear planet in planetaryGears)
        {
            if (deselect)
            {
                planet.ResetMaterial();
            }
            else
            {
                planet.SetMaterial(selectMaterial);
            }
        }
    }
    public void RebuildSystem(PlanetarySystemElement changedGear, int newCogs)
    {
        if (changedGear.gearType == GearTypePlSystem.SunGear)
        {
            sunGear.DeleteGeneratedModel();
            this.sunGearCogs = newCogs;
            sunGear.GenerateGear(newCogs);
            sunGear.ResetMaterial();
        }
        else if (changedGear.gearType == GearTypePlSystem.PlanetaryGear)
        {
            foreach (var gear in this.planetaryGears)
            {
                gear.DeleteGeneratedModel();
                this.planetGearCogs = newCogs;
                gear.GenerateGear(newCogs);
                gear.ResetMaterial();

            }
        }
        ringGear.DeleteGeneratedModel();
        ringGear.GenerateGear(sunGearCogs + (planetGearCogs * 2));
        planetaryAxis.Cogs = ringGear.Cogs;
        ringGear.ResetMaterial();
        RefreshGearPositions();

    }
    public RotatingElement GetPlanetaryAxis()
    {
        return planetaryAxis;
    }
}
