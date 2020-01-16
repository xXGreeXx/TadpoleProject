using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainCameraScript : MonoBehaviour
{
    //globals set in editor
    public GameObject brainOrigin;

    //update
    void FixedUpdate()
    {
        this.transform.LookAt(brainOrigin.transform.position);
        this.transform.RotateAround(brainOrigin.transform.position, Vector3.up, 0.5F);
    }
}
