using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools;

public class CameraMovement : MonoBehaviour
{
    //  [SerializeField] EventReference MainCave_AMBX;
    private Animator anim;
    // private TurnManager TM;
    private bool attack, buff, godcard, shield;
    private GameObject battlemusicCheck;
    public GameObject menuMusicCheck, battleMusicG;
    [SerializeField] EventReference cameraSound;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();


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

        battlemusicCheck = GameObject.Find("BattleMusic");


        if (battlemusicCheck == null)
        {
            menuMusicCheck.SetActive(true);
            battleMusicG.SetActive(false);


        }
        else
        {
            menuMusicCheck.SetActive(false);
            battleMusicG.SetActive(true);

        }


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
        bool goUp = _W && !anim.GetBool("Up");
        bool goLeft = _D && !anim.GetBool("Left");

        bool resetView = _W && anim.GetBool("Down")
                      || _A && anim.GetBool("Right")
                      || _S && anim.GetBool("Up")
                      || _D && anim.GetBool("Left");


        if (resetView) // Go to middle
        {
            ResetView();
            //SoundPlayer.Playsound(cameraSound, gameObject);

            return;
        }
        if (goDown)
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", true);
            anim.SetBool("Right", false);
            anim.SetBool("Up", false);
            anim.SetBool("Left", false);
            SoundPlayer.PlaySound(cameraSound, gameObject);

        }
        if (goLeft) // go left
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", true);
            anim.SetBool("Up", false);
            anim.SetBool("Left", false);
            SoundPlayer.PlaySound(cameraSound, gameObject);

        }
        if (goRight) // go right
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Up", false);
            anim.SetBool("Left", true);
            SoundPlayer.PlaySound(cameraSound, gameObject);

        }
        if (goUp) // go up
        {
            anim.SetBool("EnemyCloseUp", false);
            anim.SetBool("Down", false);
            anim.SetBool("Right", false);
            anim.SetBool("Up", true);
            anim.SetBool("Left", false);
            SoundPlayer.PlaySound(cameraSound, gameObject);

        }

        // Map Scene View
        if (MultiSceneLoader.getLoadedCollectionTitle.Equals("Map"))
        {
            anim.SetBool("MapCamera", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("CardReward"))
        {
            anim.SetBool("MapCamera",false);
            anim.SetBool("Up", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("HowToPlay"))
        {
            anim.SetBool("Up",false);
            anim.SetBool("MapCamera", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }
        else 
        {
            anim.SetBool("MapCamera", false);
            
        }

    }
}
