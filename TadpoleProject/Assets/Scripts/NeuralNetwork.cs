using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    //globals
    public List<GameObject> hiddenNeurons = new List<GameObject>();
    public List<GameObject> inputNeurons = new List<GameObject>();
    public List<GameObject> outputNeurons = new List<GameObject>();

    public List<GameObject> allNeurons { get { return hiddenNeurons.Concat(inputNeurons).Concat(outputNeurons).ToList(); } }

    //start
    void Start()
    {
        //create neural network
        for (int i = 0; i < GameHandler.NeuronCount; i++)
        {
            GameObject neuron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            neuron.name = "Neuron" + i;
            neuron.GetComponent<MeshRenderer>().material.color = Color.red;
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);

            neuron.transform.localPosition = new Vector3(Random.Range(-GameHandler.Size, GameHandler.Size), Random.Range(-GameHandler.Size, GameHandler.Size), Random.Range(-GameHandler.Size, GameHandler.Size));

            hiddenNeurons.Add(neuron);
        }

        float totalWidth = GameHandler.InputNeurons * 10;
        for (int i = 0; i < GameHandler.InputNeurons; i++)
        {
            GameObject neuron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            neuron.name = "InputNeuron" + i;
            neuron.GetComponent<MeshRenderer>().material.color = Color.gray;
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);

            neuron.transform.localPosition = new Vector3(-(totalWidth / 2F) + (i * 10), 0, -GameHandler.Size * 1.75F);

            inputNeurons.Add(neuron);
        }


        totalWidth = GameHandler.OutputNeurons * 10;
        for (int i = 0; i < GameHandler.OutputNeurons; i++)
        {
            GameObject neuron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            neuron.name = "OutputNeuron" + i;
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);

            neuron.transform.localPosition = new Vector3(-(totalWidth / 2F) + (i * 10), 0, GameHandler.Size * 1.75F);

            outputNeurons.Add(neuron);
        }

        //create synapses
        for (int idx = 0; idx < 100; idx++)
        {
            GameObject preSynapticNeuron = allNeurons[Random.Range(0, allNeurons.Count)];
            GameObject postSynapticNeuron = allNeurons[Random.Range(0, allNeurons.Count)];

            GameObject synapsePivot = new GameObject();
            synapsePivot.name = "SynapseHolder";
            synapsePivot.transform.SetParent(preSynapticNeuron.transform);
            synapsePivot.transform.position = preSynapticNeuron.transform.position;

            float distance = Vector3.Distance(preSynapticNeuron.transform.position, postSynapticNeuron.transform.position) - 4; //euclidian distance minus the neuron's radius * 2

            GameObject s = GameObject.CreatePrimitive(PrimitiveType.Cube);
            s.name = "Synapse";

            s.transform.SetParent(synapsePivot.transform);
            s.transform.localScale = new Vector3(0.25F, 0.25F, distance);
            s.transform.position = synapsePivot.transform.position;
            s.transform.localPosition = new Vector3(0, 0, (distance / 2F) + 2);

            synapsePivot.transform.LookAt(postSynapticNeuron.transform.position);
        }
    }

    //update
    void Update()
    {
        
    }
}
