using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCamera : MonoBehaviour
{
    private Animator anim;
    private CameraMovement cammove;

    public TurnController turnController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        cammove = Camera.main.GetComponent<CameraMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        if (turnController.state != CombatState.DrawStep)
        {
            if (cammove.cameraViewContainer == CameraView.Down)
            {
                anim.SetBool("LookDown", true);
            }
            else
            {
                anim.SetBool("LookDown", false);
            }

        }
        else
        {
            anim.SetBool("LookDown", false);
        }


    }
}
