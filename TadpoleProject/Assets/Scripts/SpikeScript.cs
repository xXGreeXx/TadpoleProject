using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeScript : MonoBehaviour
{
    //globals
    public Vector3 startPosition;
    public GameObject postSynapticObject;

    public float delayCycles = 0;
    private float time = 0;
    public float weight = 0;

    void Start()
    {
    }

    //update (called once per frame)
    void Update()
    {
        time += (Time.deltaTime * 1000);

        float t = (time / GameHandler.TickSpeed) / delayCycles;
        Vector3 newPos = Vector3.Lerp(startPosition, postSynapticObject.transform.position, t);
        this.transform.position = new Vector3(newPos.x, newPos.y, newPos.z);

        if (t >= 1)
        {
            //if (postSynapticObject.GetComponent<Neuron>() != null)
            //    postSynapticObject.GetComponent<Neuron>().PropagateSignalToAxons(0);
            //else
            //    postSynapticObject.transform.parent.GetComponent<AccumulatorScript>().AddSpike();


            Destroy(this.gameObject);
        }
    }
}
