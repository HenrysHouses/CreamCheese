// *
// * Modified by Henrik
// *
// *
// *

using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    //  [SerializeField] EventReference MainCave_AMBX;
    private Animator anim;
    // private TurnManager TM;
    private bool attack, buff, godcard, shield;
    private GameObject battlemusicCheck;
    public GameObject menuMusicCheck, battleMusicG;
    [SerializeField] EventReference cameraSound;
    public DeathCrumbling dyingCam;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        

        if(instance)
            Destroy(gameObject);
        else
            instance = this;
    }




    // Update is called once per frame
    void Update()
    {

        battlemusicCheck = GameObject.Find("BattleMusic");

       // if(dyingCam.dying)
       // {
       //     Destroy(this,1);
       //     
       // }


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

            return;
        }
        if (goDown)
        {
            LookDown();
        }
        if (goLeft) // go left
        {
            LookRight();
        }
        if (goRight) // go right
        {
            LookLeft();
        }
        if (goUp) // go up
        {
            LookUp();
        }

        // Map Scene View
        if (MultiSceneLoader.getLoadedCollectionTitle.Equals("Map"))
        {
            anim.SetBool("Up", false);
            anim.SetBool("MapCamera", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("CardReward"))
        {
            anim.SetBool("MapCamera", false);
            anim.SetBool("Up", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("RuneReward"))
        {
            anim.SetBool("MapCamera", false);
            anim.SetBool("Up", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("DeckLibrary"))
        {
            anim.SetBool("MapCamera", false);
            anim.SetBool("Up", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("HowToPlay"))
        {
            anim.SetBool("Up", false);
            anim.SetBool("HowToPlay", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }
        else
        {
            anim.SetBool("HowToPlay", false);
            anim.SetBool("MapCamera", false);

        }
    }

    public void ResetView()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
        SoundPlayer.PlaySound(cameraSound, gameObject);
    }

    public void LookRight()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", true);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
        SoundPlayer.PlaySound(cameraSound, gameObject);
    }

    public void LookLeft()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", true);
        SoundPlayer.PlaySound(cameraSound, gameObject);
    }

    public void LookDown()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", true);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
        SoundPlayer.PlaySound(cameraSound, gameObject);
    }

    public void LookUp()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", true);
        anim.SetBool("Left", false);
        SoundPlayer.PlaySound(cameraSound, gameObject);
    }

    void LookAtEnemies()
    {
        anim.SetBool("EnemyCloseUp", true);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
    }

    void LookAtCards()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", true);
        anim.SetBool("MapCamera", false);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
    }

    void LookAtMap()
    {
        anim.SetBool("EnemyCloseUp", false);
        anim.SetBool("CardCloseUp", false);
        anim.SetBool("MapCamera", true);
        anim.SetBool("Down", false);
        anim.SetBool("Right", false);
        anim.SetBool("Up", false);
        anim.SetBool("Left", false);
    }

    public void SetCameraView(CameraView view)
    {
        switch(view)
        {
            case CameraView.Reset:
                ResetView();
                break;
            case CameraView.Right:
                LookRight();
                break;
            case CameraView.Left:
                LookLeft();
                break;
            case CameraView.Down:
                LookDown();
                break;
            case CameraView.Up:
                LookUp();
                break;
            case CameraView.EnemyCloseUp:
                LookAtEnemies();
                break;
            case CameraView.CardCloseUp:
                LookAtCards();
                break;
            case CameraView.Map:
                LookAtMap();
                break;
        }
    }
}

public enum CameraView
{
    None,
    Reset,
    Right,
    Left,
    Down,
    Up,
    EnemyCloseUp,
    CardCloseUp,
    Map
}
