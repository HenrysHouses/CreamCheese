//charlie
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public Button quitButton;
    public Button resumeButton;

    // Start is called before the first frame update
    void Start()
    {
        quitButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (Time.timeScale == 1f)
                gameIsPaused = true;
            else
                gameIsPaused = false;
            
            PauseGame();
            Cursor.visible = true;
        }
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            resumeButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            resumeButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(0);
    }
}
