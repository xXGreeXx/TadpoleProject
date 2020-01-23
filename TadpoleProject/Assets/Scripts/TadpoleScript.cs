using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TadpoleScript : MonoBehaviour
{
    //objects set in editor
    public GameObject[] tailObjects;
    public GameObject[] lineObjects;

    //raycast data
    private float raycastRange = 30;
    private float degreesOfSeparation = 0.1F;

    //brain
    private NeuralNetwork brain;

    //start
    void Start()
    {
        //get brain
        brain = GameObject.Find("BrainOrigin").GetComponent<NeuralNetwork>();
    }

    //update
    void Update()
    {
        //get tadpole inputs from raycaster
        float[] inputs = fetchInputsFromRayCaster();

        //propagate ray data into input neurons
        for (int i = 0; i < inputs.Length; i++)
        {
            brain.inputNeurons[i].GetComponent<InputScript>().firingRate = (Mathf.Min(inputs[i], raycastRange) / raycastRange);
        }

        //fetch outputs and calculate deltas
        float[] outputs = new float[brain.outputNeurons.Count];
        for (int i = 0; i < brain.outputNeurons.Count; i++)
        {
            outputs[i] = brain.outputNeurons[i].GetComponent<OutputScript>().potential;
        }
        float[] deltas = new float[outputs.Length / 2];

        for (int i = 0; i < outputs.Length; i += 2)
        {
            deltas[i / 2] = outputs[i] - outputs[i + 1];
        }

        //tail motor control
        for (int i = 0; i < 5; i++)
        {
            HingeJoint2D joint = tailObjects[i].GetComponent<HingeJoint2D>();

            JointMotor2D motor = joint.motor;
            motor.motorSpeed = deltas[i] * 300;

            if (joint.jointAngle > joint.limits.min && joint.jointAngle < joint.limits.max)
            {
                tailObjects[i].GetComponent<ConstantForce2D>().relativeForce = new Vector2(0, Mathf.Abs(motor.motorSpeed) * 5 * (5F / (float)(i + 1F)));
            }
            else
            {
                tailObjects[i].GetComponent<ConstantForce2D>().relativeForce = Vector2.zero;

                if (motor.motorSpeed == 0) //TODO\\ change this?
                    motor.motorSpeed = joint.jointAngle < 0 ? 30 : -30;
            }

            joint.motor = motor;
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
            Vector2 direction = (Vector2)(transform.up) - Vector2.Lerp(transform.right * 1.25F, -transform.right * 1.25F, angle);

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

            lineObjects[i].GetComponent<LineRenderer>().SetPosition(0, new Vector3(pos.x, pos.y, 110.7F));
            lineObjects[i].GetComponent<LineRenderer>().SetPosition(1, new Vector3(hitPoint.x, hitPoint.y, 110.7F));
        }

        //return data
        return outputData.ToArray();
    }
}
