using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //objects listed in editor
    public GameObject[] inputFields;
    public GameObject[] outputFields;
    public GameObject brainOrigin;

    //global neural network data
    public static int HiddenNeuronCount = 150;
    public static int Size = 100;
    public static int InputNeurons = 10;
    public static int OutputNeurons = 10;

    public const float TickSpeed = 1000F;
    public static float time = 0; //EVERYTHING THAT USES THIS VARIABLE MUST BE IN FIXEDUPDATE

    //visualizer data
    private const float originalHeight = 45;

    //start
    void Start()
    {

    }

    //update
    void FixedUpdate()
    {
        time += (Time.deltaTime * 1000);
        if (time >= TickSpeed + 25)
            time = 0;

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
