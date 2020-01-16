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
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);

            neuron.transform.localPosition = new Vector3(Random.Range(-GameHandler.Size, GameHandler.Size), Random.Range(-GameHandler.Size, GameHandler.Size), Random.Range(-GameHandler.Size, GameHandler.Size));
        }
    }

    //update
    void Update()
    {
        
    }
}
