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
    public GameObject GamePanel;

    public GameObject graph1;
    public GameObject graph2;
    public GameObject graph3;
    public GameObject graph4;
    public GameObject graph5;

    public GameObject lead1Object;
    public GameObject lead2Object;
    public GameObject lead3Object;
    public GameObject lead4Object;
    public GameObject lead5Object;

    public GameObject foodPrefab;

    //global settings
    public const float Spike_Value = 1;
    public const float Output_Decay = 2F;
    public const int SodiumInfluxRate = 50;
    public const int PotassiumOutfluxRate = 50;

    public const int BasePotassiumCount = 180;
    public const int BaseSodiumCount = 0;
    public const int BaseChlorideCount = 250;

    //global neural network data
    public static int HiddenNeuronCount = 300;
    public static int Size = 100;
    public static int InputNeurons = 10;
    public static int OutputNeurons = 10;

    //global simulation data
    public const float TickSpeed = 500F;
    public static float time = 0; //EVERYTHING THAT USES THIS VARIABLE MUST BE IN FIXEDUPDATE

    //game data
    public static List<GameObject> food = new List<GameObject>();
    public const int FoodCount = 30;

    //visualizer data
    private const float originalHeight = 45;
    private float[] lead1 = new float[75];
    private float[] lead2 = new float[75];
    private float[] lead3 = new float[75];
    private float[] lead4 = new float[75];
    private float[] lead5 = new float[75];
    private int idx = 0;
    private float readTime = 0;

    //enums
    public enum SynapseType
    {
        Excitatory,
        Inhibitory,
        Modulatory
    }

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

        //spawn food
        for (int i = 0; i < FoodCount; i++)
        {
            CreateFoodObject();
        }
    }

    //spawn food
    private void CreateFoodObject()
    {
        GameObject foodObject = GameObject.Instantiate(foodPrefab);
        foodObject.transform.SetParent(GamePanel.transform);

        foodObject.transform.localScale = Vector3.one;
        foodObject.name = "Food";
        foodObject.transform.localPosition = new Vector2(Random.Range(-575, 575), Random.Range(-500, 500));
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
                lead1[idx] = CalculateLead(lead1Object);
                lead2[idx] = CalculateLead(lead2Object);
                lead3[idx] = CalculateLead(lead3Object);
                lead4[idx] = CalculateLead(lead4Object);
                lead5[idx] = CalculateLead(lead5Object);
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
                lead1[lead1.Length - 1] = CalculateLead(lead1Object);
                lead2[lead1.Length - 1] = CalculateLead(lead2Object);
                lead3[lead1.Length - 1] = CalculateLead(lead3Object);
                lead4[lead1.Length - 1] = CalculateLead(lead4Object);
                lead5[lead1.Length - 1] = CalculateLead(lead5Object);
            }

            Grapher.ClearGraph(graph1);
            Grapher.GraphToPanel(graph1, lead1, lead1.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph2);
            Grapher.GraphToPanel(graph2, lead2, lead2.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph3);
            Grapher.GraphToPanel(graph3, lead3, lead3.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph4);
            Grapher.GraphToPanel(graph4, lead4, lead4.Max(x => x + 300), 50);

            Grapher.ClearGraph(graph5);
            Grapher.GraphToPanel(graph5, lead5, lead5.Max(x => x + 300), 50);

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

    //calculate lead data
    private float CalculateLead(GameObject lead)
    {
        float averagePotential = 0;

        float max = brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons.Max(x => Vector3.Distance(x.transform.position, lead.transform.position));
        foreach (GameObject neuron in brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons)
        {
            averagePotential += neuron.GetComponent<Neuron>().MembranePotential * (1 - (Vector3.Distance(lead.transform.position, neuron.transform.position) / max));
        }
        averagePotential /= brainOrigin.GetComponent<NeuralNetwork>().hiddenNeurons.Count;

        return averagePotential;
    }
}
