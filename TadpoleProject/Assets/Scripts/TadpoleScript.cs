using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleScript : MonoBehaviour
{
    //objects set in editor
    public GameObject[] tailObjects;

    private float[] speeds = { 0, 0, 0, 0, 0 };
    private bool[] switchs = { false, false, false, false, false };

    //start
    void Start()
    {

    }

    //update
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            JointMotor2D motor = tailObjects[i].GetComponent<HingeJoint2D>().motor;
            motor.motorSpeed = (switchs[i] ? -1 : 1) * 60;
            tailObjects[i].GetComponent<HingeJoint2D>().motor = motor;
            tailObjects[i].GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, Mathf.Abs(motor.motorSpeed) * 20);

            speeds[i] += switchs[i] ? -1 : 1;

            switchs[i] = Input.GetKey(KeyCode.A);

            //if (speeds[i] >= 20)
            //    switchs[i] = true;
            //else if (speeds[i] <= -60)
            //    switchs[i] = false;
        }
    }
}
