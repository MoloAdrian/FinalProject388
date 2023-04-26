using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class FlowManager : MonoBehaviour
{

    static public bool isPaused = false;
    public Canvas canvas;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPause()
    {
        Debug.Log("paused");
        Time.timeScale = 0.0f;
        isPaused = true;
        canvas.transform.Find("PauseMenu").gameObject.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1.0f;

        canvas.transform.Find("PauseMenu").gameObject.SetActive(false);

    }
    public void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
