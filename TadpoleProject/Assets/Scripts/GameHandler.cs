using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //objects listed in editor
    public GameObject[] inputFields;
    public GameObject brainOrigin;

    //global neural network data
    public static int HiddenNeuronCount = 150;
    public static int Size = 100;
    public static int InputNeurons = 10;
    public static int OutputNeurons = 10;

    public const float TickSpeed = 1000F;

    //visualizer data
    private const float originalHeight = 45;

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        if (brainOrigin != null && brainOrigin.GetComponent<NeuralNetwork>() != null)
        {
            NeuralNetwork brain = brainOrigin.GetComponent<NeuralNetwork>();

            int idx = 0;
            foreach (GameObject input in brain.inputNeurons)
            {
                inputFields[idx].GetComponent<RectTransform>().sizeDelta = new Vector2(10, originalHeight - ((input.GetComponent<InputScript>().membranePotential / 10F) * originalHeight));

                idx++;
            }
        }
    }
}
