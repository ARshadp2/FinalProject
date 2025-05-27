using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AIBehavior : Agent
{
    public int current_episode = 0;
    public float cumulative_reward = 0f;
    private float speed = 10f;
    private float current_time = 0;

    void Update() {
        if ((int) Time.time == Time.time)
            AddReward(1f);
        if (Time.time - current_time >= 60) {
            cumulative_reward = GetCumulativeReward();
            EndEpisode();
        }
        int count = 0;
        for (float x = Mathf.PI/3; x < Mathf.PI * 2; x += Mathf.PI/3) {
            RaycastHit hit;
            int distance = 5;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, new Vector3(Mathf.Cos(x), 0, Mathf.Sin(x)), out hit, distance) && hit.collider.tag == "bullet")

            { 
                Debug.DrawRay(transform.position, new Vector3(Mathf.Cos(x), 0, Mathf.Sin(x)) * distance, Color.yellow); 
                //Debug.Log("Did Hit " + x); 
            }
            else
            { 
                Debug.DrawRay(transform.position, new Vector3(Mathf.Cos(x), 0, Mathf.Sin(x)) * distance, Color.white); 
                //Debug.Log("Did not Hit " + x); 
            }
        } 
    }
    public override void Initialize() {
        current_episode = 0;
        cumulative_reward = 0f;
    }

    public override void OnEpisodeBegin() {
        current_episode++;
        cumulative_reward = 0f;
        transform.localPosition = new Vector3(0f, 0f, 0f);
        current_time = Time.time;
    }

    public override void CollectObservations(VectorSensor sensor) {
        /*
        int count = 0;
        for (float x = Mathf.PI/3; x < Mathf.PI * 2; x += Mathf.PI/3) {
            RaycastHit hit;
            int distance = 5;
            if (Physics.Raycast(transform.position, new Vector3(Mathf.Cos(x), 0, Mathf.Sin(x)), out hit, distance) && hit.collider.tag == "bullet") {
                count++;    
            }
        }
        */
        float pos_x = transform.localPosition.x / 15;
        float pos_z = transform.localPosition.z / 15;
        //count = (count - 3) / 3;
        //Debug.Log(pos_x + "");
        //Debug.Log(pos_z + "");
        //Debug.Log(count + " count");
        sensor.AddObservation(pos_x);
        sensor.AddObservation(pos_z);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity.x);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity.z);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        MoveAgent(actions.DiscreteActions);
        //AddReward(-2f/MaxStep);
        cumulative_reward = GetCumulativeReward();
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))
            discreteActionsOut[0] = 2;
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))
            discreteActionsOut[0] = 8;
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))
            discreteActionsOut[0] = 4;
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow))
            discreteActionsOut[0] = 6;
        else if (Input.GetKey(KeyCode.UpArrow))
            discreteActionsOut[0] = 1;
        else if (Input.GetKey(KeyCode.RightArrow))
            discreteActionsOut[0] = 3;
        else if (Input.GetKey(KeyCode.DownArrow))
            discreteActionsOut[0] = 5;
        else if (Input.GetKey(KeyCode.LeftArrow))
            discreteActionsOut[0] = 7;
    }

    public void MoveAgent(ActionSegment<int> act) {
        var action = act[0];
        switch (action) {
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 45, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 4:
                transform.rotation = Quaternion.Euler(0, 135, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 5:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 6:
                transform.rotation = Quaternion.Euler(0, 225, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 7:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
            case 8:
                transform.rotation = Quaternion.Euler(0, 315, 0);
                transform.position += transform.forward * speed * Time.deltaTime;
                break;
        }
    }
    void OnTriggerEnter(Collider other) {
        Debug.Log("yes1");
        if (other.tag == "bullet") {
            AddReward(-10f);
        }
        cumulative_reward = GetCumulativeReward();
    }
}
