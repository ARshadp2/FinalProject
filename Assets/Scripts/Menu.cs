using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    void Update() {
        Cursor.visible = true;
    }
    // Start is called before the first frame update
    public void OnStartButton() {
        SceneManager.LoadScene("SampleScene");
    }
    public void OnQuitButton() {
        Application.Quit();
    }
   /* public void OnHowToButton() {
        SceneManager.LoadScene("Instructions Menu");
    } */
}

