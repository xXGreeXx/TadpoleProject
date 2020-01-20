using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour, Assets.Scripts.INode
{
    //globals
    public List<GameObject> outputAxons = new List<GameObject>();
    private List<GameObject> neighbors = new List<GameObject>();

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        
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
