using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.SubtractScore(1);
                Destroy(gameObject);
            }
        }
        if (other.CompareTag("AI"))
        {
            AIScore ascore = FindObjectOfType<AIScore>();
            if (ascore != null)
            {
                ascore.SubtractScore(1);
                Destroy(gameObject);
            }
        }
    }
}
