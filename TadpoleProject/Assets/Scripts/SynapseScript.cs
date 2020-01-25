using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynapseScript : MonoBehaviour
{
    //globals
    public float weight;
    public float fatigue = 0; //synaptic fatigue
    public float delay;
    public GameHandler.SynapseType type;

    public GameObject preSynapticNeuron;
    public GameObject postSynapticNeuron;

    void FixedUpdate()
    {
        //vesicle modulation
        if (fatigue > 0)
        {
            fatigue -= Time.deltaTime;
            fatigue = Mathf.Max(fatigue, 0);
        }

        //STDP
        if (postSynapticNeuron.GetComponent<Neuron>() == null || preSynapticNeuron.GetComponent<Neuron>() == null) //don't do STDP on output synapses or input synapses
            return;

        if (postSynapticNeuron.GetComponent<Neuron>().PotassiumGateOpen)
        {
            if (preSynapticNeuron.GetComponent<Neuron>().PotassiumGateOpen)
                weight += 0.1F;
            else
                weight -= 0.1F;

            if (weight <= 0)
            {
                preSynapticNeuron.GetComponent<Neuron>().outputAxons.Remove(this.gameObject);
                Destroy(this.gameObject);
            }
        }
    }

    //propagate data onto synapse
    public void PropagateSpike()
    {
        GameObject spike = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        spike.name = "Spike";
        //spike.transform.SetParent(this.transform);
        spike.transform.localScale = new Vector3(2, 2, 2);

        spike.transform.position = preSynapticNeuron.transform.position;

        spike.GetComponent<MeshRenderer>().material.color = Color.yellow;

        SpikeScript script = spike.AddComponent<SpikeScript>();
        script.startPosition = preSynapticNeuron.transform.position;
        script.postSynapticObject = postSynapticNeuron;
        script.delayCycles = 0.5F; //TODO\\ change this too
        script.weight = weight - fatigue; //TODO\\ use vesicle data to calculate this

        fatigue += 0.5F;
    }
}
