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
    public bool Refractory = false;

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
        if (Refractory)
        {
            PotassiumCount += 2;
            SodiumCount -= 3;

            if (SodiumCount <= 0)
            {
                SodiumCount = 0;

                if (PotassiumCount <= 130)
                {
                    PotassiumCount++;
                }
                else
                    Refractory = false;
            }
        }

        //after all potassium is out of the cell, then start pumping it back in
        if (PotassiumCount <= 0)
        {
            PotassiumCount = 0;
            PotassiumGateOpen = false;
            Refractory = true;
        }

        //activate sodium gates to create action potential
        if (MembranePotential >= SodiumGateActivation && !Refractory && !PotassiumGateOpen)
        {
            SodiumGateOpen = true;
        }

        //deactive sodium gate and activate potassium gate and propaget signal to axons
        if (MembranePotential >= PotassiumGateActivation && !Refractory)
        {
            PotassiumGateOpen = true;
            SodiumGateOpen = false;
            PropagateSignalToAxons(0);
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
