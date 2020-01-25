using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    //globals
    public Vector3 startPosition;
    public GameObject postSynapticObject;

    public float delayCycles = 0;
    public float weight = 0;

    public GameHandler.SynapseType type;

    void Start()
    {
    }

    //update (called once per frame)
    void Update()
    {
        float t = (GameHandler.time / GameHandler.TickSpeed) / delayCycles;
        Vector3 newPos = Vector3.Lerp(startPosition, postSynapticObject.transform.position, t);
        this.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);

        if (t >= 1)
        {
            if (postSynapticObject.GetComponent<Neuron>() != null)
            {
                if (!postSynapticObject.GetComponent<Neuron>().Refractory)
                {
                    switch (type)
                    {
                        case GameHandler.SynapseType.Excitatory:
                            postSynapticObject.GetComponent<Neuron>().SodiumCount += Mathf.Max((int)(weight * 15), 0);
                            break;

                        case GameHandler.SynapseType.Inhibitory:
                            postSynapticObject.GetComponent<Neuron>().PotassiumCount -= Mathf.Max((int)(weight * 15), 0);
                            break;

                        case GameHandler.SynapseType.Modulatory:
                            postSynapticObject.GetComponent<Neuron>().ModulatorCount += Mathf.Max((int)(weight * 15), 0);
                            break;
                    }
                }
            }
            else
                postSynapticObject.GetComponent<OutputScript>().potential = 1;


            Destroy(this.gameObject);
        }
    }
}
