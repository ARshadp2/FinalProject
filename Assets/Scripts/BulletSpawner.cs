using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public bool time_picked;
    public float time_mark;
    public float start_time;
    // Start is called before the first frame update
    void Start() {
        time_mark = 1;
        time_picked = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (time_mark > 0) {
            time_picked = true;
        }
        else {
            time_mark = UnityEngine.Random.Range(1, 1.5f);
            time_picked = false;
        }
        if (time_picked == true && start_time + time_mark <= Time.time) {
            time_picked = false;
            time_mark = 0;
            start_time = Time.time;
            float x = UnityEngine.Random.Range(-30f, 30f);
            float y = Mathf.Sqrt(900f-x*x);
            if ((int) Time.time % 2 == 0) {
                y = -y;
            }
            GameObject proj = Instantiate(bullet, transform.position + new Vector3(x, 0, y), Quaternion.identity);
            proj.transform.LookAt(transform.position);
            if (UnityEngine.Random.Range(0f, 1f) > .5) {
                GameObject projectile = Instantiate(bullet, transform.position + new Vector3(x + 2, 0, y + 2), Quaternion.identity);
                projectile.transform.LookAt(transform.position);
            }
            if (UnityEngine.Random.Range(0f, 1f) > .5) {
                GameObject projectile = Instantiate(bullet, transform.position + new Vector3(x - 2, 0, y - 2), Quaternion.identity);
                projectile.transform.LookAt(transform.position);
            }
        }
    }
}
