using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance {private set; get;}

    public Animator Animator {private set; get;}
    Canvas canvas;
    public static bool IsAnimating {private set; get;}

    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        Animator = GetComponent<Animator>();
    
        if(Instance)
            Destroy(gameObject);
        else
            Instance = this;
    }


    public IEnumerator EnterLoadingScreen()
    {
        IsAnimating = true;
        
        SceneManager.sceneLoaded += FinishLoadingScreen;

        Animator.Play("EnterLoading");
        canvas.worldCamera = Camera.main;
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
        IsAnimating = false;
    }

    private void FinishLoadingScreen(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(ExitLoadingScreen());
        Debug.Log("scene loaded");
        SceneManager.sceneLoaded -= FinishLoadingScreen;
    }

    public IEnumerator ExitLoadingScreen()
    {
        IsAnimating = true;
        Animator.Play("ExitLoading");
        canvas.worldCamera = Camera.main;
        yield return new WaitForSeconds(Animator.GetCurrentAnimatorStateInfo(0).length);
        IsAnimating = false;
    }
}
