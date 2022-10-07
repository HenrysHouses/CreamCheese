using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Animator anim;
    private GameManager GM;
    private bool lookingUp,lookingDown,lookingRight,lookingLeft;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.Log(MultiSceneLoader.getLoadedCollectionTitle);


        if(Input.GetKeyDown(KeyCode.W) && !lookingDown)
        {
            anim.SetBool("Up", true);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Left", false);
            lookingUp = true;
            lookingDown = false;
            lookingLeft = false;
            lookingRight = false;
        }
        else if(Input.GetKeyDown(KeyCode.W) && lookingDown)
        {
            anim.SetBool("Down", false);
             lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;
        }

         if(Input.GetKeyDown(KeyCode.D) && !lookingLeft)
        {
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", true);
            anim.SetBool("Left", false);
            lookingRight = true;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;
            
        }
         else if(Input.GetKeyDown(KeyCode.D) && lookingLeft)
         {
            anim.SetBool("Left",false);
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;

        }
        
        

         if(Input.GetKeyDown(KeyCode.A) && !lookingRight)
        {
            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Left", true);
            lookingLeft = true;
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
        }
        else if(Input.GetKeyDown(KeyCode.A) && lookingRight)
        {
            anim.SetBool("Right", false);
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;

        }

            if(Input.GetKeyDown(KeyCode.S) && !lookingUp)
        {
            anim.SetBool("Up", false);
            anim.SetBool("Down", true);
            anim.SetBool("Right", false);
            anim.SetBool("Left", false);
            lookingDown = true;
            lookingLeft = false;
            lookingRight = false;
            lookingUp = false;
        }
        else if(Input.GetKeyDown(KeyCode.S) && lookingUp)
        {
            anim.SetBool("Up", false);
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;
        }


        if(MultiSceneLoader.getLoadedCollectionTitle.Equals("Map"))
        {
            anim.SetBool("MapCamera", true);
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;
        }
        else 
        {
            anim.SetBool("MapCamera", false);
        }
        
    }
}
