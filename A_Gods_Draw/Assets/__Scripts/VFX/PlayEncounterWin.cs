using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CameraShaker;

public class PlayEncounterWin : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator anim;
    public BoardStateController boarstate;
    private float timer;
    private Camera cam;
    void Start()
    {
       anim = GetComponentInChildren<Animator>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(boarstate.isEnemyDefeated)
        {
            timer += Time.deltaTime;
            anim.SetBool("WinEncounter", true);
            
            

            
        }
        if(timer > 5)
        {
            anim.SetBool("WinEncounter", false);
            timer = 0;
        }
       
    }
}
