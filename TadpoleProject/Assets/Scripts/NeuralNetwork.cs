using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts;

public class NeuralNetwork : MonoBehaviour
{
    //debug
    private const bool random = true;

    //globals
    public List<GameObject> hiddenNeurons = new List<GameObject>();
    public List<GameObject> inputNeurons = new List<GameObject>();
    public List<GameObject> outputNeurons = new List<GameObject>();

    public List<GameObject> allNeurons { get { return hiddenNeurons.Concat(inputNeurons).Concat(outputNeurons).ToList(); } }

    //start
    void Start()
    {
        //create neurons
        float x = -GameHandler.Size;
        float y = -GameHandler.Size;
        float z = -GameHandler.Size;
        float increment = 16.6F;
        for (int i = 0; i < GameHandler.HiddenNeuronCount; i++)
        {
            GameObject neuron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            neuron.name = "Neuron" + i;
            neuron.GetComponent<MeshRenderer>().material.color = Color.red;
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);
            neuron.AddComponent<Neuron>();

            if(random)
                neuron.transform.localPosition = new Vector3(Random.Range(-GameHandler.Size, GameHandler.Size), Random.Range(-GameHandler.Size, GameHandler.Size), Random.Range(-GameHandler.Size, GameHandler.Size));
            else
            {
                neuron.transform.localPosition = new Vector3(x, y, z);

                x += increment;

                if(x > GameHandler.Size)
                {
                    x = -GameHandler.Size;
                    y += increment;

                    if(y > GameHandler.Size)
                    {
                        y = -GameHandler.Size;
                        z += increment;
                    }
                }
            }

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
            neuron.AddComponent<InputScript>();

            neuron.transform.localPosition = new Vector3(-(totalWidth / 2F) + (i * 10), 0, -GameHandler.Size * 1.25F);

            inputNeurons.Add(neuron);
        }


        totalWidth = GameHandler.OutputNeurons * 10;
        for (int i = 0; i < GameHandler.OutputNeurons; i++)
        {
            GameObject neuron = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            neuron.name = "OutputNeuron" + i;
            neuron.transform.SetParent(this.transform);
            neuron.transform.localScale = new Vector3(2, 2, 2);
            neuron.AddComponent<OutputScript>();

            neuron.transform.localPosition = new Vector3(-(totalWidth / 2F) + (i * 10), 0, GameHandler.Size * 1.25F);

            outputNeurons.Add(neuron);
        }

        //find neighbors
        foreach (GameObject n in allNeurons)
        {
            if (n.GetComponent<Neuron>() != null) //only interneurons and input neurons have neighbors
                n.GetComponent<Neuron>().setNeighbors(getNeighbors(n.transform.position));
            else if (n.GetComponent<InputScript>() != null)
                n.GetComponent<InputScript>().setNeighbors(getNeighbors(n.transform.position));
        }

        //create synapses
        for (int idx = 0; idx < 400; idx++)
        {
            GameObject preSynapticNeuron = allNeurons.Except(outputNeurons).ToList()[Random.Range(0, allNeurons.Except(outputNeurons).ToList().Count)];
            INode preSynapticNeuronScript = (preSynapticNeuron.GetComponent<Neuron>() != null ? (INode)preSynapticNeuron.GetComponent<Neuron>() : (preSynapticNeuron.GetComponent<InputScript>() != null ? (INode)preSynapticNeuron.GetComponent<InputScript>() : null));
            if (preSynapticNeuronScript == null) //don't create outgoing connections from accumulator nodes(output)
                continue;

            GameObject postSynapticNeuron = preSynapticNeuronScript.getNeighbors()[Random.Range(0, preSynapticNeuronScript.getNeighbors().Count)];

            GameObject synapsePivot = new GameObject();
            synapsePivot.name = "SynapseHolder";
            synapsePivot.transform.SetParent(preSynapticNeuron.transform);
            synapsePivot.transform.position = preSynapticNeuron.transform.position;

            float distance = Vector3.Distance(preSynapticNeuron.transform.position, postSynapticNeuron.transform.position);

            GameObject s = GameObject.CreatePrimitive(PrimitiveType.Cube);
            s.name = "Synapse";

            s.transform.SetParent(synapsePivot.transform);
            s.transform.localScale = new Vector3(0.25F, 0.25F, distance);
            s.transform.position = synapsePivot.transform.position;
            s.transform.localPosition = new Vector3(0, 0, (distance / 2F));

            SynapseScript script = s.AddComponent<SynapseScript>();
            script.preSynapticNeuron = preSynapticNeuron;
            script.postSynapticNeuron = postSynapticNeuron;
            script.delay = 1;

            synapsePivot.transform.LookAt(postSynapticNeuron.transform.position);

            preSynapticNeuron.GetComponent<INode>().AddAxon(s);
        }
    }

    //update
    void Update()
    {
        
    }

    //get neighbors
    private List<GameObject> getNeighbors(Vector3 position)
    {
        List<GameObject> neighbors = new List<GameObject>();

        List<GameObject> sortedObjects = allNeurons.Except(inputNeurons).OrderBy(x => Vector3.Distance(position, x.transform.position)).ToList();

        neighbors = sortedObjects.GetRange(1, 30);

        return neighbors;
    }
}
