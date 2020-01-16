using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleScript : MonoBehaviour
{
    //objects set in editor
    public GameObject[] tailObjects;
    public GameObject[] lineObjects;

    //data for tail movement
    private float[] speeds = { 0, 0, 0, 0, 0 };
    private bool[] switchs = { false, false, false, false, false };

    //raycast data
    private float raycastRange = 30;
    private float degreesOfSeparation = 0.1F;

    //start
    void Start()
    {

    }

    //update
    void Update()
    {
        //get tadpole inputs from raycaster
        float[] inputs = fetchInputsFromRayCaster();

        //tail motor control
        for (int i = 0; i < 5; i++)
        {
            JointMotor2D motor = tailObjects[i].GetComponent<HingeJoint2D>().motor;
            motor.motorSpeed = (switchs[i] ? -1 : 1) * 100;
            tailObjects[i].GetComponent<HingeJoint2D>().motor = motor;
            tailObjects[i].GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, Mathf.Abs(motor.motorSpeed) * 15 * (5F / (float)(i + 1F)));

            speeds[i] += switchs[i] ? -1 : 1;

            switchs[i] = Input.GetKey(KeyCode.A);

            //if (speeds[i] >= 20)
            //    switchs[i] = true;
            //else if (speeds[i] <= -60)
            //    switchs[i] = false;
        }
    }

    //fetch network inputs from raycaster
    private float[] fetchInputsFromRayCaster()
    {
        //cast rays
        List<float> outputData = new List<float>();

        float angle = 0;
        for (int i = 0; i < lineObjects.Length; i++)
        {
            //calculate ray
            Vector2 pos = transform.position + (transform.up * 1.25F);
            Vector2 direction = (Vector2)(transform.up) - Vector2.Lerp(transform.right * 1.5F, -transform.right * 1.5F, angle);

            Ray2D ray = new Ray2D(pos, direction);
            RaycastHit2D result = Physics2D.Raycast(pos, direction, raycastRange);

            Vector3 hitPoint;
            if (result.collider != null)
                hitPoint = result.point;
            else
                hitPoint = ray.GetPoint(raycastRange);


            outputData.Add(Vector2.Distance(pos, hitPoint)); //add distance to output data

            angle += degreesOfSeparation;

            //draw ray
            lineObjects[i].GetComponent<LineRenderer>().startColor = Color.red;
            lineObjects[i].GetComponent<LineRenderer>().endColor = Color.red;
            lineObjects[i].GetComponent<LineRenderer>().startWidth = 0.1F;
            lineObjects[i].GetComponent<LineRenderer>().endWidth = 0.1F;

            lineObjects[i].GetComponent<LineRenderer>().SetPosition(0, new Vector3(pos.x, pos.y, 110));
            lineObjects[i].GetComponent<LineRenderer>().SetPosition(1, new Vector3(hitPoint.x, hitPoint.y, 110));
        }

        //return data
        return outputData.ToArray();
    }
}
