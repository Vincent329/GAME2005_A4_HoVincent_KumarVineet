using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPerson : MonoBehaviour
{
    private bool controllerEnabled = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Take 'P' for Pause
        if (Input.GetKeyDown("p"))
        {
            controllerEnabled = !controllerEnabled;
            //Debug.Log("Paused");
        }

        // Pause the game
        if (!controllerEnabled)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Gun.sharedInstance.inPauseState = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Gun.sharedInstance.inPauseState = false;
        }

        // Disable player controller on P press
        GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = controllerEnabled;

        //Take Q button for going back to Main Menu
        if (Input.GetKeyDown("q"))
        {
            Debug.Log("Q Button Pressed");
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
            }
        }

    }
}
