using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    //objects listed in editor
    public GameObject[] inputFields;
    public GameObject[] outputFields;
    public GameObject brainOrigin;

    public GameObject graph1;
    public GameObject graph2;
    public GameObject graph3;
    public GameObject graph4;
    public GameObject graph5;

    public GameObject linePrefab;

    //global settings
    public const float Spike_Value = 1;
    public const float Output_Decay = 2F;
    public const int SodiumInfluxRate = 10;
    public const int PotassiumOutfluxRate = 10;

    //global neural network data
    public static int HiddenNeuronCount = 100;
    public static int Size = 100;
    public static int InputNeurons = 10;
    public static int OutputNeurons = 10;

    //global simulation data
    public const float TickSpeed = 500F;
    public static float time = 0; //EVERYTHING THAT USES THIS VARIABLE MUST BE IN FIXEDUPDATE

    //visualizer data
    private const float originalHeight = 45;
    private float[] lead1 = new float[50];
    private int idx = 0;

    //start
    void Start()
    {
        //set all leads to zero
        for (int i = 0; i < lead1.Length; i++)
            lead1[i] = 0;
    }

    //update
    void FixedUpdate()
    {
        //main clock
        time += (Time.deltaTime * 1000);
        if (time >= TickSpeed + 25)
        {
            time = 0;
        }

        //visualize leads every 20 internal cycles
        if (time % 30 == 0)
        {
            if (idx < lead1.Length)
            {
                lead1[idx] = brainOrigin.GetComponent<NeuralNetwork>().inputNeurons[0].GetComponent<InputScript>().membranePotential;
                idx++;
            }
            else
            {
                for (int idx = 1; idx < lead1.Length; idx++)
                {
                    lead1[idx - 1] = lead1[idx];
                }
                lead1[lead1.Length - 1] = brainOrigin.GetComponent<NeuralNetwork>().inputNeurons[0].GetComponent<InputScript>().membranePotential;
            }

            Grapher.ClearGraph(graph1);
            Grapher.GraphToPanel(graph1, lead1, linePrefab);
        }

        //visualize inputs and outputs
        if (brainOrigin != null && brainOrigin.GetComponent<NeuralNetwork>() != null)
        {
            NeuralNetwork brain = brainOrigin.GetComponent<NeuralNetwork>();

            int idx = 0;
            foreach (GameObject input in brain.inputNeurons)
            {
                inputFields[idx].GetComponent<RectTransform>().sizeDelta = new Vector2(10, originalHeight - ((input.GetComponent<InputScript>().membranePotential / 10F) * originalHeight));

                idx++;
            }

            idx = 0;
            foreach (GameObject output in brain.outputNeurons)
            {
                outputFields[idx].GetComponent<RectTransform>().sizeDelta = new Vector2(10, originalHeight - ((output.GetComponent<OutputScript>().potential) * originalHeight));

                idx++;
            }
        }
    }
}
