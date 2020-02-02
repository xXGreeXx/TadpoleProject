using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class Neuron : MonoBehaviour, Assets.Scripts.INode
{
    //globals
    public List<GameObject> outputAxons = new List<GameObject>(); //axons that receive signals from this neuron
    private List<GameObject> neighbors = new List<GameObject>(); //neurons that this neuron CAN link to during synaptogenesis

    //neuron data
    public int PotassiumCount = GameHandler.BasePotassiumCount; //flows out of membrane
    public int SodiumCount = GameHandler.BaseSodiumCount; //flows into membrane
    public int ChlorideCount = GameHandler.BaseChlorideCount; //constant in membrane
    public int ModulatorCount = 0; //secondary messenger that modulates threshold value

    public float MembranePotential { get { return PotassiumCount + SodiumCount - ChlorideCount; } } //calculate membrane potential

    public float SodiumGateActivation = -55; //mV that activates sodium gates
    public float PotassiumGateActivation = 40; //mV that activates potassium gates

    public bool SodiumGateOpen = false; //when this is true, sodium floods into cell causing mV to rise
    public bool PotassiumGateOpen = false; //when this is true, potassium floods out of cell causing mV to fall
    public bool Refractory = false; //when this is true, Na+/K+ ATPase pumps activate to bring sodium out of the cell and potassium into the cell. Potassium leak gates also open

    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        //drain secondary messengers
        if (ModulatorCount > 0)
        {
            ModulatorCount--;

            ModulatorCount = Mathf.Max(ModulatorCount, 0);
        }

        //bring sodium and potassium into the cell
        if(SodiumGateOpen)
        {
            SodiumCount += GameHandler.SodiumInfluxRate;
        }
        if (PotassiumGateOpen)
        {
            PotassiumCount -= GameHandler.PotassiumOutfluxRate;
        }
        if (Refractory)
        {
            PotassiumCount += 12;
            SodiumCount -= 18;

            if (SodiumCount <= 0)
            {
                SodiumCount = 0;

                if (PotassiumCount <= GameHandler.BasePotassiumCount)
                {
                    PotassiumCount += 5;

                    if (PotassiumCount > GameHandler.BasePotassiumCount)
                        PotassiumCount = GameHandler.BasePotassiumCount;
                }
                else
                    Refractory = false;
            }
        }

        //leak sodium out of the cell
        if (!SodiumGateOpen && SodiumCount > 0)
        {
            SodiumCount -= 1;
        }

        //after all potassium is out of the cell, then start pumping it back in
        if (PotassiumCount <= 0)
        {
            PotassiumCount = 0;
            PotassiumGateOpen = false;
            Refractory = true;
        }

        //activate sodium gates to create action potential
        if (MembranePotential >= (SodiumGateActivation - ModulatorCount) && !Refractory && !PotassiumGateOpen)
        {
            SodiumGateOpen = true;
        }

        //deactive sodium gate and activate potassium gate and propaget signal to axons
        if (MembranePotential >= PotassiumGateActivation && !Refractory)
        {
            PotassiumGateOpen = true;
            SodiumGateOpen = false;
            PropagateSignalToAxons(0);
        }
    }

    //add axon to list
    public void AddAxon(GameObject axon)
    {
        outputAxons.Add(axon);
    }

    //fire neuron
    public void PropagateSignalToAxons(int offset)
    {
        //propagate signal
        foreach (GameObject axon in outputAxons)
        {
            axon.GetComponent<SynapseScript>().PropagateSpike();
        }

        //connect to a random neuron
        INode preSynapticNeuronScript = (this.GetComponent<Neuron>() != null ? (INode)this.GetComponent<Neuron>() : (this.GetComponent<InputScript>() != null ? (INode)this.GetComponent<InputScript>() : null));

        //get data
        GameObject postSynapticNeuron = preSynapticNeuronScript.getNeighbors()[Random.Range(0, preSynapticNeuronScript.getNeighbors().Count)];

        GameObject synapsePivot = new GameObject();
        synapsePivot.name = "SynapseHolder";
        synapsePivot.transform.SetParent(this.transform);
        synapsePivot.transform.position = this.transform.position;

        float distance = Vector3.Distance(this.transform.position, postSynapticNeuron.transform.position);

        //create object
        GameObject s = GameObject.CreatePrimitive(PrimitiveType.Cube);
        s.name = "Synapse";

        s.transform.SetParent(synapsePivot.transform);
        s.transform.localScale = new Vector3(0.25F, 0.25F, distance);
        s.transform.position = synapsePivot.transform.position;
        s.transform.localPosition = new Vector3(0, 0, (distance / 2F));

        //set synapse data
        SynapseScript script = s.AddComponent<SynapseScript>();
        script.preSynapticNeuron = this.gameObject;
        script.postSynapticNeuron = postSynapticNeuron;
        script.delay = 1;
        script.weight = 1;
        script.type = GameHandler.SynapseType.Excitatory;

        //rotate synapse
        synapsePivot.transform.LookAt(postSynapticNeuron.transform.position);

        this.GetComponent<INode>().AddAxon(s);
    }

    //encapsulate neighbors for interface
    public List<GameObject> getNeighbors()
    {
        return neighbors;
    }

    public void setNeighbors(List<GameObject> neighbors)
    {
        this.neighbors = neighbors;
    }
}
