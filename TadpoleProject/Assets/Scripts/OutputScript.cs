using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputScript : MonoBehaviour
{
    //globals
    public float potential = 0;

    //Update
    void Update()
    {
        if(potential > 0)
        {
            potential -= Time.deltaTime * GameHandler.Output_Decay;
            potential = Mathf.Max(potential, 0);
        }
    }
}
