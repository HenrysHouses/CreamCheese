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
    public Button quitButton;
    public Button resumeButton, optionsButton, backButton;
    public Slider master_SFX_Slider, music_Slider, SFX_Slider;
    public GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        quitButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        panel.SetActive(false);

        master_SFX_Slider.gameObject.SetActive(false);
        SFX_Slider.gameObject.SetActive(false);
        music_Slider.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
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
            panel.SetActive(true);
            resumeButton.gameObject.SetActive(true);
            quitButton.gameObject.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            panel.SetActive(false);
            resumeButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
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
        Time.timeScale = 0;
        resumeButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(false);

        master_SFX_Slider.gameObject.SetActive(true);
        SFX_Slider.gameObject.SetActive(true);
        music_Slider.gameObject.SetActive(true);

    }

    public void backFromOptions()
    {
        resumeButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);

        master_SFX_Slider.gameObject.SetActive(false);
        SFX_Slider.gameObject.SetActive(false);
        music_Slider.gameObject.SetActive(false);
        backButton.gameObject.SetActive(false);
    }
}
