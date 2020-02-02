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
        foreach (GameObject axon in outputAxons)
        {
            axon.GetComponent<SynapseScript>().PropagateSpike();
        }

        membranePotential = 10;

        //connect to a random neuron
        INode preSynapticNeuronScript = (this.GetComponent<Neuron>() != null ? (INode)this.GetComponent<Neuron>() : (this.GetComponent<InputScript>() != null ? (INode)this.GetComponent<InputScript>() : null));

        //get data
        GameObject postSynapticNeuron = preSynapticNeuronScript.getNeighbors()[Random.Range(0, preSynapticNeuronScript.getNeighbors().Count)];

        GameObject synapsePivot = new GameObject();
        synapsePivot.name = "SynapseHolder";
        synapsePivot.transform.SetParent(this.transform);
        synapsePivot.transform.position = this.transform.position;

        float distance = Vector3.Distance(this.transform.position, postSynapticNeuron.transform.position);

        //create object
        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Cube);
        s.name = "Synapse";

        s.transform.SetParent(synapsePivot.transform);
        s.transform.localScale = new Vector3(0.25F, 0.25F, distance);
        s.transform.position = synapsePivot.transform.position;
        s.transform.localPosition = new Vector3(0, 0, (distance / 2F));

        //set synapse data
        SynapseScript script = s.AddComponent<SynapseScript>();
        script.preSynapticNeuron = this.gameObject;
        script.postSynapticNeuron = postSynapticNeuron;
        script.delay = 1;
        script.weight = 1;
        script.type = GameHandler.SynapseType.Excitatory;

        //rotate synapse
        synapsePivot.transform.LookAt(postSynapticNeuron.transform.position);

        this.GetComponent<INode>().AddAxon(s);
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
