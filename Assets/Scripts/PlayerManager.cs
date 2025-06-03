using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;
    void Start() {
        transform.localPosition = new Vector3(-10, 0f, 0f);
    }
    void Update() {
        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow)) {
            transform.rotation = Quaternion.Euler(0, 45, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow)) {
            transform.rotation = Quaternion.Euler(0, 315, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow)) {
            transform.rotation = Quaternion.Euler(0, 135, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.LeftArrow)) {
            transform.rotation = Quaternion.Euler(0, 225, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.UpArrow)) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow)) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.rotation = Quaternion.Euler(0, 270, 0);
            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
    }
}