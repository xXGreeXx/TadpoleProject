using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour, Assets.Scripts.INode
{
    //globals
    public List<GameObject> outputAxons = new List<GameObject>();
    private List<GameObject> neighbors = new List<GameObject>();

    //neuron data
    public int PotassiumCount = 130;
    public int SodiumCount = 0;
    public int ChlorideCount = 200;
    public float MembranePotential { get { return PotassiumCount + SodiumCount - ChlorideCount; } }

    public float SodiumGateActivation = -55;
    public float PotassiumGateActivation = 40;

    public bool SodiumGateOpen = false;
    public bool PotassiumGateOpen = false;

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        //bring sodium and potassium into the cell
        if(SodiumGateOpen)
        {
            SodiumCount += GameHandler.SodiumInfluxRate;
        }
        if (PotassiumGateOpen)
        {
            PotassiumCount -= GameHandler.PotassiumOutfluxRate;
        }

        //activate sodium gates to create action potential
        if (MembranePotential > SodiumGateActivation)
        {
            //PropagateSignalToAxons(0);
            SodiumGateOpen = true;
        }

        //deactive sodium gate and activate potassium gate
        if (MembranePotential > PotassiumGateActivation)
        {
            PotassiumGateOpen = true;
            SodiumGateOpen = false;
        }
    }

    //add axon to list
    public void AddAxon(GameObject axon)
    {
        outputAxons.Add(axon);
    }

    //propagate signal to output axons
    public void PropagateSignalToAxons(int offset)
    {
        foreach (GameObject axon in outputAxons)
        {
            axon.GetComponent<SynapseScript>().PropagateSpike();
        }
    }

    //encapsulate neighbors for interface
    public List<GameObject> getNeighbors()
    {
        return neighbors;
    }

    public void setNeighbors(List<GameObject> neighbors)
    {
        this.neighbors = neighbors;
    }
}
