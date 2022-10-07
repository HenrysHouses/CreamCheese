using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Animator anim;
    private GameManager GM;
    private TurnManager TM;
    private bool lookingUp,lookingDown,lookingRight,lookingLeft;
    private bool attack, buff, godcard, shield;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void SelectCardCamera()
    {
            anim.SetBool("EnemyCloseUp", true);
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;
            Debug.Log("ANimatincardplayed");
    }

    void DeselectCamera()
    {
            anim.SetBool("EnemyCloseUp", false);
            lookingRight = false;
            lookingUp = false;
            lookingDown = false;
            lookingLeft = false;
            Debug.Log("Does it move?");
        
    }
    // Update is called once per frame
    void Update()
    {
        if(TM == null && MultiSceneLoader.getLoadedCollectionTitle.Equals("Combat"))
        {
             GameObject G = GameObject.Find("TurnManager");
           if(G)
           {
                TM = G.GetComponent<TurnManager>();
                //TM.OnSelectedCard.AddListener(SelectCardCamera);
                //TM.OnDeSelectedCard.AddListener(DeselectCamera);
           } 
        }
        
//        Debug.Log(MultiSceneLoader.getLoadedCollectionTitle);


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
