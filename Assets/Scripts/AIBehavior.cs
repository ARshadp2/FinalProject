using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AIBehavior : Agent
{
    public LayerMask layermask;
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
        Collider[] hitColliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(transform.localPosition, 10, hitColliders, layermask);
        float[] distances = new float[10];
        for (int x = 0; x < hitColliders.Length; x++) {
            if (hitColliders[x] == null) {
                distances[x] = 0;
            }
            else
                distances[x] = Mathf.Abs(Vector3.Distance(transform.localPosition, hitColliders[x].transform.localPosition));
        }
        Array.Sort(distances, hitColliders);
        Array.Reverse(hitColliders);
        for (int x = 0; x < 3; x++) {
            if (hitColliders[x] == null) {
                Debug.Log("none");
            }
            else {
                float bullet_x = hitColliders[x].transform.localPosition.x - transform.localPosition.x;
                float bullet_z = hitColliders[x].transform.localPosition.z - transform.localPosition.z;
                float vel_x = hitColliders[x].GetComponent<Rigidbody>().velocity.x;
                float vel_z = hitColliders[x].GetComponent<Rigidbody>().velocity.z;
                float projectile_angle = hitColliders[x].transform.eulerAngles.y;
                if (projectile_angle > 180)
                    projectile_angle -= 360;
                float angle = Mathf.Abs(Mathf.Atan(bullet_z/bullet_x) * Mathf.Rad2Deg - projectile_angle);
                //Debug.Log(bullet_x + " x");
                //Debug.Log(bullet_z + " z");
                //Debug.Log(vel_x + " vel_x");
                //Debug.Log(vel_z + " vel_z");
                Debug.Log(angle + " angle change");
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
        Collider[] hitColliders = new Collider[10];
        Physics.OverlapSphereNonAlloc(transform.localPosition, 10, hitColliders, layermask);
        float[] distances = new float[10];
        for (int x = 0; x < hitColliders.Length; x++) {
            if (hitColliders[x] == null) {
                distances[x] = 0;
            }
            else
                distances[x] = Mathf.Abs(Vector3.Distance(transform.localPosition, hitColliders[x].transform.localPosition));
        }
        Array.Sort(distances, hitColliders);
        Array.Reverse(hitColliders);
        for (int x = 0; x < 3; x++) {
            if (hitColliders[x] == null) {
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
            }
            else {
                float bullet_x = (hitColliders[x].transform.localPosition.x - transform.localPosition.x) / 15f;
                float bullet_z = (hitColliders[x].transform.localPosition.z - transform.localPosition.z) / 15f;
                float vel_x = hitColliders[x].GetComponent<Rigidbody>().velocity.x / 15f;
                float vel_z = hitColliders[x].GetComponent<Rigidbody>().velocity.z / 15f;
                float projectile_angle = hitColliders[x].transform.eulerAngles.y;
                if (projectile_angle > 180)
                    projectile_angle -= 360;
                float angle = Mathf.Abs(Mathf.Atan(bullet_z/bullet_x) * Mathf.Rad2Deg - projectile_angle) / 180;
                sensor.AddObservation(bullet_x);
                sensor.AddObservation(bullet_z);
                sensor.AddObservation(vel_x);
                sensor.AddObservation(vel_z);
                sensor.AddObservation(angle);
            }
        }

        float pos_x = transform.localPosition.x / 15;
        float pos_z = transform.localPosition.z / 15;
        
        sensor.AddObservation(pos_x);
        sensor.AddObservation(pos_z);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity.x / 15f);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity.z / 15f);
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
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 2:
                transform.rotation = Quaternion.Euler(0, 45, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 3:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 4:
                transform.rotation = Quaternion.Euler(0, 135, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 5:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 6:
                transform.rotation = Quaternion.Euler(0, 225, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 7:
                transform.rotation = Quaternion.Euler(0, 270, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
                break;
            case 8:
                transform.rotation = Quaternion.Euler(0, 315, 0);
                transform.localPosition += transform.forward * speed * Time.deltaTime;
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
