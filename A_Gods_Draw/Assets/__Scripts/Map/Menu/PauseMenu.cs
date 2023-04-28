//charlie
using Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HH.MultiSceneTools;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused;
    public Button resumeButton, optionsButton, backButton, quitButton;
    public GameObject sliders, panel;

    private float timeScale;

    // Start is called before the first frame update
    void Start()
    {
        quitButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        panel.SetActive(false);

        sliders.SetActive(false);
        backButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        timeScale = Time.timeScale;

        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (timeScale == 1f)
            {
                gameIsPaused = true;
            }
            else
            {
                gameIsPaused = false;
                sliders.SetActive(false);
                backButton.gameObject.SetActive(false);
            }

            PauseGame();
            Cursor.visible = true;
        }
    }

    public void PauseGame()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
            panel.SetActive(true);
            resumeButton.gameObject.SetActive(true);
            optionsButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
            GameManager.instance.PauseMenuIsOpen = true;
        }
        else
        {
            Time.timeScale = 1;
            panel.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
            GameManager.instance.PauseMenuIsOpen = false;
        }
    }

    public void QuitGame()
    {
        MultiSceneLoader.loadCollection("MainMenu", collectionLoadMode.Difference);
        Time.timeScale = 1;
        panel.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
    }


    public void OptionsMenu()
    {
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);

        sliders.SetActive(true);
        backButton.gameObject.SetActive(true);
    }

    public void backFromOptions()
    {
        resumeButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);

        sliders.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
}
