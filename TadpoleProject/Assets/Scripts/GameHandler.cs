using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public const int SodiumInfluxRate = 50;
    public const int PotassiumOutfluxRate = 50;

    public const int BasePotassiumCount = 180;
    public const int BaseSodiumCount = 0;
    public const int BaseChlorideCount = 250;

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
    private float[] lead1 = new float[75];
    private float[] lead2 = new float[75];
    private float[] lead3 = new float[75];
    private float[] lead4 = new float[75];
    private float[] lead5 = new float[75];
    private int idx = 0;
    private float readTime = 0;

    //start
    void Start()
    {
        //set all leads to zero
        for (int i = 0; i < lead1.Length; i++)
        {
            lead1[i] = -70;
            lead2[i] = -70;
            lead3[i] = -70;
            lead4[i] = -70;
            lead5[i] = -70;
        }
    }

    //update
    void FixedUpdate()
    {
        //main clock
        time += (Time.deltaTime * 1000);
        readTime += Time.deltaTime * 1000;
        if (time >= TickSpeed + 5)
        {
            time = 0;
        }

        //visualize leads
        if (readTime >= 25)
        {
            if (idx < lead1.Length)
            {
                lead1[idx] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[0].GetComponent<Neuron>().MembranePotential;
                lead2[idx] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[1].GetComponent<Neuron>().MembranePotential;
                lead3[idx] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[2].GetComponent<Neuron>().MembranePotential;
                lead4[idx] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[3].GetComponent<Neuron>().MembranePotential;
                lead5[idx] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[4].GetComponent<Neuron>().MembranePotential;
                idx++;
            }
            else
            {
                for (int idx = 1; idx < lead1.Length; idx++)
                {
                    lead1[idx - 1] = lead1[idx];
                    lead2[idx - 1] = lead2[idx];
                    lead3[idx - 1] = lead3[idx];
                    lead4[idx - 1] = lead4[idx];
                    lead5[idx - 1] = lead5[idx];
                }
                lead1[lead1.Length - 1] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[0].GetComponent<Neuron>().MembranePotential;
                lead2[lead1.Length - 1] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[1].GetComponent<Neuron>().MembranePotential;
                lead3[lead1.Length - 1] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[2].GetComponent<Neuron>().MembranePotential;
                lead4[lead1.Length - 1] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[3].GetComponent<Neuron>().MembranePotential;
                lead5[lead1.Length - 1] = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons[4].GetComponent<Neuron>().MembranePotential;
            }

            Grapher.ClearGraph(graph1);
            Grapher.GraphToPanel(graph1, lead1, linePrefab, lead1.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph2);
            Grapher.GraphToPanel(graph2, lead2, linePrefab, lead2.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph3);
            Grapher.GraphToPanel(graph3, lead3, linePrefab, lead3.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph4);
            Grapher.GraphToPanel(graph4, lead4, linePrefab, lead4.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph5);
            Grapher.GraphToPanel(graph5, lead5, linePrefab, lead5.Max(x => x + 300), 50);

            readTime = 0;
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
