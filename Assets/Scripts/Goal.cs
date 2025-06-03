using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private ScoreManager scoreManager;
    private AIScore ascore;

    void Start()
    {
        transform.localPosition = new Vector3(5, 0, 0);

        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Move goal to new random position
            transform.localPosition = new Vector3(
                UnityEngine.Random.Range(-15, 15), 
                0, 
                UnityEngine.Random.Range(-15, 15)
            );

            // Add a point to the score
            if (scoreManager != null)
            {
                scoreManager.AddScore(1);
            }


        }
        if (other.CompareTag("AI"))
        {
            // Move goal to new random position
            transform.localPosition = new Vector3(
                UnityEngine.Random.Range(-15, 15), 
                0, 
                UnityEngine.Random.Range(-15, 15)
            );

            // Add a point to the score
            if (ascore != null)
            {
                ascore.AddScore(1);
            }
        }
    }
}
