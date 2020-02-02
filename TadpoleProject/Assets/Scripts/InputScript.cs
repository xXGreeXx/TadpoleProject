using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
public class InputScript : MonoBehaviour, Assets.Scripts.INode
{
    //globals
    public List<GameObject> outputAxons = new List<GameObject>();
    private List<GameObject> neighbors = new List<GameObject>();

    private int fireCount = 0;

    public float firingRate = 1;
    public float membranePotential = 0;

    //update (occurs after gamehandler)
    void FixedUpdate()
    {
        float t = (GameHandler.time / GameHandler.TickSpeed);

        if (t >= firingRate && fireCount < 1F / firingRate)
        {
            PropagateSignalToAxons(0);
            fireCount++;
        }
        if (t == 0)
            fireCount = 0;

        if (membranePotential > 0)
        {
            membranePotential -= 20 * Time.deltaTime;
            membranePotential = membranePotential < 0 ? 0 : membranePotential;
        }
    }

    //propagate signal to output axons
    public void PropagateSignalToAxons(int offset)
    {
        if (outputAxons.Count < GameHandler.MaxConnectionPerNeuron)
        {
            INode preSynapticNeuronScript = (this.GetComponent<Neuron>() != null ? (INode)this.GetComponent<Neuron>() : (this.GetComponent<InputScript>() != null ? (INode)this.GetComponent<InputScript>() : null));
            GameObject postSynapticNeuron = preSynapticNeuronScript.getNeighbors()[Random.Range(0, preSynapticNeuronScript.getNeighbors().Count)];

            GameHandler.brain.CreateConnection(this.gameObject, postSynapticNeuron);
        }

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

    //get axons
    public List<GameObject> GetAxons()
    {
        return outputAxons;
    }
}
