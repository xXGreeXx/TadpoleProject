using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork : MonoBehaviour
{
    //globals
    public List<GameObject> neurons = new List<GameObject>();

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
        }


        totalWidth = GameHandler.OutputNeurons * 10;
        for (int i = 0; i < GameHandler.OutputNeurons; i++)
        {
            GameObject neuron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            neuron.name = "OutputNeuron" + i;
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);

            neuron.transform.localPosition = new Vector3(-(totalWidth / 2F) + (i * 10), 0, GameHandler.Size * 1.75F);
        }
    }

    //update
    void Update()
    {
        
    }
}
