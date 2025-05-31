using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public bool time_picked;
    public float time_mark;
    public float start_time;
    public bool sweep_right;
    public float cool_down;
    public float cool_down_start;
    public Vector3 og;
    // Start is called before the first frame update
    void Start() {
        time_mark = 1;
        time_picked = true;
        sweep_right = true;
        cool_down_start = 0;
        cool_down = 0;
        og = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        cool_down = Time.time - cool_down_start;
        if (sweep_right == true)
            transform.position += transform.right * 10 * Time.deltaTime;
        else
            transform.position -= transform.right * 10 * Time.deltaTime;
        if (Vector3.Distance(transform.position, og) >= 15)
            if (sweep_right == true)
                sweep_right = false;
            else
                sweep_right = true;
        if (time_mark > 0) {
            time_picked = true;
        }
        else {
            time_mark = UnityEngine.Random.Range(.25f, .5f);
            time_picked = false;
        }
        if (time_picked == true && start_time + time_mark <= Time.time) {
            time_picked = false;
            time_mark = 0;
            start_time = Time.time;
            cool_down_start = Time.time;
            GameObject proj = Instantiate(bullet, transform.position, transform.rotation);
        }
    }
}
