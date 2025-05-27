using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float angle;
    public float speed;
    public float start_time;
    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(new Vector3(0,0,0));
        angle = UnityEngine.Random.Range(-30f, 30f);
        transform.Rotate(90, angle, 0);
        speed = UnityEngine.Random.Range(5f, 15f);
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        if (Time.time - start_time > 10 || (int) Time.time % 60 == 0)
            Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other) {
        Debug.Log("yes2");
        if (other.tag == "Player") {
            Destroy(gameObject);
        }
    }
}
