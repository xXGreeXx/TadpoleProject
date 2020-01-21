using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour, Assets.Scripts.INode
{
    //globals
    public List<GameObject> outputAxons = new List<GameObject>();
    private List<GameObject> neighbors = new List<GameObject>();

    private float time = 0;
    private float firingRate = 1;

    public float membranePotential = 0;

    //update
    void Update()
    {
        time += (Time.deltaTime * 1000);

        float t = (time / GameHandler.TickSpeed);

        if (t >= firingRate)
        {
            PropagateSignalToAxons(0);

            time = 0;
        }

        if (membranePotential > 0)
        {
            membranePotential -= 20 * Time.deltaTime;
            membranePotential = membranePotential < 0 ? 0 : membranePotential;
        }
    }

    //propagate signal to output axons
    public void PropagateSignalToAxons(int offset)
    {
        foreach (GameObject axon in outputAxons)
        {
            axon.GetComponent<SynapseScript>().PropagateSpike();
        }

        membranePotential = 10;
    }

    //add axon to list
    public void AddAxon(GameObject axon)
    {
        outputAxons.Add(axon);
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
