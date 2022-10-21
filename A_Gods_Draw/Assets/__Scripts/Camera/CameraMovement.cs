using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CameraMovement : MonoBehaviour
{
  //  [SerializeField] EventReference MainCave_AMBX;
    private Animator anim;
    // private TurnManager TM;
    private bool attack, buff, godcard, shield;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
      //  SoundPlayer.Playsound(MainCave_AMBX, gameObject);
    }

    void SelectCardCamera()
    {
        anim.SetBool("EnemyCloseUp", true);
        anim.SetBool("Down", true);
        anim.SetBool("Right", true);
        anim.SetBool("Up", true);
        anim.SetBool("Left", true);
        anim.Play("EnemyCloseup");
    }

    void ResetView()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
    }
    // Update is called once per frame
    void Update()
    {
        // if(TM == null && MultiSceneLoader.getLoadedCollectionTitle.Equals("Combat"))
        // {
        //      GameObject G = GameObject.Find("TurnManager");
        //    if(G)
        //    {
        //         TM = G.GetComponent<TurnManager>();
        //         TM.OnSelectedAttackCard.AddListener(SelectCardCamera);
        //         TM.OnDeSelectedAttackCard.AddListener(ResetView);
        //    } 
        // }
        
        bool _W = Input.GetKeyDown(KeyCode.W);
        bool _A = Input.GetKeyDown(KeyCode.A);
        bool _S = Input.GetKeyDown(KeyCode.S);
        bool _D = Input.GetKeyDown(KeyCode.D);

        bool goDown = _S && !anim.GetBool("Down");
        bool goRight = _A && !anim.GetBool("Right");
        bool goUp =  _W && !anim.GetBool("Up");
        bool goLeft = _D && !anim.GetBool("Left");
        
        bool resetView = _W && anim.GetBool("Down") 
                      || _A && anim.GetBool("Right") 
                      || _S && anim.GetBool("Up") 
                      || _D && anim.GetBool("Left");

        
        if(resetView) // Go to middle
        {
            ResetView();
            return;
        }
        if(goDown)
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", true);
            anim.SetBool("Right", false);
            anim.SetBool("Up", false);
            anim.SetBool("Left", false);
        }
        if(goLeft) // go left
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", true);
            anim.SetBool("Up", false);
            anim.SetBool("Left", false);
        }
        if(goRight) // go right
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Up", false);
            anim.SetBool("Left", true);
        }
        if(goUp) // go up
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Up", true);
            anim.SetBool("Left", false);
        }

        // Map Scene View
        if(MultiSceneLoader.getLoadedCollectionTitle.Equals("Map"))
        {
            anim.SetBool("MapCamera", true);
        }
        else 
        {
            anim.SetBool("MapCamera", false);
        }
        
    }
}
