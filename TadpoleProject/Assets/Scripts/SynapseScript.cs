using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynapseScript : MonoBehaviour
{
    public float weight;
    public float delay;

    public GameObject preSynapticNeuron;
    public GameObject postSynapticNeuron;

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
        script.delayCycles = 1;
        script.weight = 0; //TODO\\ use vesicle data to calculate this
    }
}
