using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public bool time_picked;
    public float time_mark;
    public float start_time;
    public int sweep_right;
    public float start_angle;
    public float cool_down;
    public float cool_down_start;
    // Start is called before the first frame update
    void Start() {
        time_mark = 1;
        time_picked = true;
        sweep_right = 1;
        start_angle = transform.eulerAngles.y;
        cool_down_start = 0;
        cool_down = 0;
    }
    // Update is called once per frame
    void Update()
    {
        cool_down = Time.time - cool_down_start;
        transform.Rotate(0, 30 * sweep_right * Time.deltaTime, 0);
        float angle_current = transform.eulerAngles.y;
        if (transform.localPosition == new Vector3(0, 0, -20) && angle_current <= 360 && angle_current >= 230)
            angle_current -= 360;
        if (angle_current >= start_angle + 30) {
            sweep_right = -1;
        }
        if (angle_current <= start_angle - 30) {
            sweep_right = 1;
        }
        if (time_mark > 0) {
            time_picked = true;
        }
        else {
            time_mark = UnityEngine.Random.Range(1f, 2f);
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
