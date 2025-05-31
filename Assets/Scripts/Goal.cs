using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(5, 0, 0);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            transform.localPosition = new Vector3(UnityEngine.Random.Range(-15, 15), 0, UnityEngine.Random.Range(-15, 15));
        }
    }
}
