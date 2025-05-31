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
    public GameObject spawner1;
    public GameObject goal;
    public List<Collider> in_range = new List<Collider>();
    public int current_episode = 0;
    public float cumulative_reward = 0f;
    private float speed = 10f;
    private float current_time = 0;
    private int hits = 0;
    public Vector3 og;
    void Start() {
        og = transform.position;
    }
    void Update() {
        
        if ((int) Time.time / 10 == Time.time) {
            AddReward(.1f * (10 - hits));
        }
        if (Time.time - current_time >= 60) {
            cumulative_reward = GetCumulativeReward();
            EndEpisode();
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
        hits = 0;
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
        Array.Reverse(distances);
        for (int x = 0; x < 5; x++) {
            if (distances[x] != 0) {
                float distancer = distances[x];
                if ((int) Time.time / 10 == Time.time)
                    AddReward(distancer * .01f);
            }
        }
        for (int x = 0; x < 3; x++) {
            if (hitColliders[x] == null) {
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
                sensor.AddObservation(0);
            }
            else {
                float bullet_x = (hitColliders[x].transform.localPosition.x - transform.localPosition.x) / 30f;
                float bullet_z = (hitColliders[x].transform.localPosition.z - transform.localPosition.z) / 30f;
                float vel_x = hitColliders[x].GetComponent<Rigidbody>().velocity.x / 15f;
                float vel_z = hitColliders[x].GetComponent<Rigidbody>().velocity.z / 15f;
                sensor.AddObservation(bullet_x);
                sensor.AddObservation(bullet_z);
                sensor.AddObservation(vel_x);
                sensor.AddObservation(vel_z);
                sensor.AddObservation(distances[x] / 30);
                sensor.AddObservation(Vector3.Dot(hitColliders[x].transform.forward, (transform.position - hitColliders[x].transform.position).normalized));
            }
        }

        float pos_x = transform.localPosition.x / 30;
        float pos_z = transform.localPosition.z / 30;
        AddReward(Vector3.Distance(transform.position, goal.transform.position) / -30f);
        sensor.AddObservation(transform.position.x - goal.transform.position.x);
        sensor.AddObservation(transform.position.z - goal.transform.position.z);
        sensor.AddObservation(pos_x);
        sensor.AddObservation(pos_z);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity.x / 15f);
        sensor.AddObservation(GetComponent<Rigidbody>().velocity.z / 15f);
        /*
        Collider[] hit1 = Physics.OverlapSphere(new Vector3(og.x, 0, og.y), 5, layermask);
        Collider[] hit2 = Physics.OverlapSphere(new Vector3(og.x + 7.5f, 0, og.y), 5, layermask);
        Collider[] hit3 = Physics.OverlapSphere(new Vector3(og.x - 7.5f, 0, og.y), 5, layermask);
        Collider[] hit4 = Physics.OverlapSphere(new Vector3(og.x, 0, og.y + 7.5f), 5, layermask);;
        Collider[] hit5 = Physics.OverlapSphere(new Vector3(og.x, 0, og.y - 7.5f), 5, layermask);;
        Collider[] hit6 = Physics.OverlapSphere(new Vector3(og.x + 7.5f, 0, og.y + 7.5f), 5, layermask);
        Collider[] hit7 = Physics.OverlapSphere(new Vector3(og.x - 7.5f, 0, og.y - 7.5f), 5, layermask);
        Collider[] hit8 = Physics.OverlapSphere(new Vector3(og.x + 7.5f, 0, og.y - 7.5f), 5, layermask);
        Collider[] hit9 = Physics.OverlapSphere(new Vector3(og.x - 7.5f, 0, og.y + 7.5f), 5, layermask);
        
        AddReward((1 - hit1.Length/10f) * .01f);
        AddReward((1 - hit2.Length/10f) * .01f);
        AddReward((1 - hit3.Length/10f) * .01f);
        AddReward((1 - hit4.Length/10f) * .01f);
        AddReward((1 - hit5.Length/10f) * .01f);
        AddReward((1 - hit6.Length/10f) * .01f);
        AddReward((1 - hit7.Length/10f) * .01f);
        AddReward((1 - hit8.Length/10f) * .01f);
        AddReward((1 - hit9.Length/10f) * .01f);
        */
        sensor.AddObservation((spawner1.transform.localPosition.x - transform.localPosition.x) / 30f);
        sensor.AddObservation((spawner1.transform.localPosition.z - transform.localPosition.z) / 30f);

        sensor.AddObservation(Vector3.Dot(spawner1.transform.forward, (transform.position - spawner1.transform.position).normalized));

        sensor.AddObservation(spawner1.GetComponent<BulletSpawner>().cool_down/2);

        for (int x = 0; x < in_range.Count; x++) {
            if (!Physics.OverlapSphere(transform.position, 10, layermask).ToList().Contains(in_range[x]) && (int) Time.time == Time.time)
                AddReward(.01f);
        }
        
        Collider[] check = Physics.OverlapSphere(transform.position, 5f, layermask);
        for (int x = 0; x < check.Length; x++) {
            if (!in_range.Contains(check[x]))
                in_range.Add(check[x]);
            if ((int) Time.time == Time.time) {
                float angle = Vector3.Dot(spawner1.transform.forward, (transform.position - spawner1.transform.position).normalized);
                AddReward(-.1f * (angle - .3f));
            }
        }
        
    }

    public override void OnActionReceived(ActionBuffers actions) {
        MoveAgent(actions.DiscreteActions);
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

        if (discreteActionsOut[0] == 0 && Physics.OverlapSphere(transform.position, 5, layermask).Length == 0)
            AddReward(0.01f);
        if (discreteActionsOut[0] == 0 && Physics.OverlapSphere(transform.position, 5, layermask).Length != 0)
            AddReward(-0.01f);
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
        
        Collider[] all_bullets = Physics.OverlapSphere(transform.position, 30f, layermask);
        for (int x = 0; x < all_bullets.Length; x++) {
            if (transform.position.z <= all_bullets[x].transform.position.z + 2 && (int) Time.time / 10 == Time.time) {
                if (transform.position.x >= all_bullets[x].transform.position.z) {
                    if (action == 1)
                        AddReward(-1f);
                    else if (action == 2)
                        AddReward(-.25f);
                    else if (action == 3)
                        AddReward(.5f);
                    else if (action == 4)
                        AddReward(1f);
                    else if (action == 5)
                        AddReward(.5f);
                    else if (action == 6)
                        AddReward(.25f);
                    else if (action == 7)
                        AddReward(-.25f);
                    else if (action == 8)
                        AddReward(-.75f);
                    else
                        AddReward(-.25f);
                }
                else {
                    if (action == 1)
                        AddReward(-1f);
                    else if (action == 2)
                        AddReward(-.75f);
                    else if (action == 3)
                        AddReward(-.25f);
                    else if (action == 4)
                        AddReward(.25f);
                    else if (action == 5)
                        AddReward(.5f);
                    else if (action == 6)
                        AddReward(1f);
                    else if (action == 7)
                        AddReward(.5f);
                    else if (action == 8)
                        AddReward(-.25f);
                    else
                        AddReward(-.25f);
                }
            }
        }
        
    }
    void OnTriggerEnter(Collider other) {
        if (other.tag == "bullet") {
            AddReward(-10f);
            in_range.Remove(other);
            hits++;
        }
        if (other.tag == "goal") {
            AddReward(.5f * (10 - hits));
        }
    }
}
