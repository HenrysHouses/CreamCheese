// *
// * Modified by Henrik
// * Modified By Snackolay, sorry
// *
// *

/*using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools;

public class CameraMovementNonAnims : MonoBehaviour
{

    [SerializeField]
    CamPos[] _CamPossies;
    public static CameraMovement instance;
    //  [SerializeField] EventReference MainCave_AMBX;
    // private TurnManager TM;
    private bool looking,hasSetCamera;
    private GameObject battlemusicCheck;
    public GameObject menuMusicCheck, battleMusicG;
    [SerializeField] EventReference cameraSound;
    private CamPos target, previusPos;
    float lerptime;
    // Start is called before the first frame update
    void Start()
    {


        if (instance)
            Destroy(gameObject);
        else
            instance = this;


        target = _CamPossies[0];
        previusPos = _CamPossies[0];

    }





    public void ResetView()
    {

        SetCameraView(CameraView.Reset);

    }

    public void LookAtCards()
    {
        SetCameraView(CameraView.Down);
    }

    public void LookUp()
    {
        SetCameraView(CameraView.Reset);
    }

    public void LookLeft()
    {
        SetCameraView(CameraView.Left);
    }

    public void LookRight()
    {
        SetCameraView(CameraView.Right);
    }

    public void LookAtMap()
    {
        SetCameraView(CameraView.Map);
    }

    public void EnemyCloseUp()
    {
        SetCameraView(CameraView.EnemyCloseUp);
    }

    public void RuneBeforePick()
    {
        SetCameraView(CameraView.RuneBeforePick);
    }

    public void RuneAfterPick()
    {
        SetCameraView(CameraView.RuneAfterPick);
    }

    public void CardReward()
    {
        SetCameraView(CameraView.CardReward);
    }


    void Update()
    {
        
        hasSetCamera = lerptime >= 1 ? true : false;

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

        lerptime += Time.deltaTime * 2.5f;

        transform.position = Vector3.Lerp(previusPos.cameraPos, target.cameraPos, lerptime);
        transform.rotation = Quaternion.Lerp(previusPos.cameraRot, target.cameraRot, lerptime);


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



        bool goDown = Input.GetKeyDown(KeyCode.S);
        bool goRight = Input.GetKeyDown(KeyCode.D);
        bool goUp = Input.GetKeyDown(KeyCode.W);
        bool goLeft = Input.GetKeyDown(KeyCode.A);
        bool goMap = MultiSceneLoader.getLoadedCollectionTitle.Equals("Map");
        bool GoReward = MultiSceneLoader.getLoadedCollectionTitle.Equals("CardReward");
        //bool resetView = goLeft && Input.GetKeyDown(KeyCode.D) || goRight && Input.GetKeyDown(KeyCode.A);


       
        if (goDown)
        {
            if(hasSetCamera)
            LookAtCards();
            

        }
        if (goLeft) // go left
        {
            if(hasSetCamera)
            LookLeft();

        }
        if (goRight) // go right
        {
            if(hasSetCamera)
            LookRight();


        }
        if (goUp) // go up
        {
            ResetView();

        }
        if(GoReward)
        {
            CardReward();
        }

       // if (resetView)
       // {
       //     ResetView();
       // }

        if (goMap)
        {
            LookAtMap();

        }

        


        // Map Scene View
        // if (MultiSceneLoader.getLoadedCollectionTitle.Equals("Map"))
        // {
        //     if (!hasSetCamera)
        //     {
        //         SetCameraView(CameraView.Map);
        //         hasSetCamera = true;
        //         if (lerptime <= 1)
        //         {
        //             hasSetCamera = false;
        //         }
        //     }
        //     //anim.SetBool("Up", false);
        //     //anim.SetBool("MapCamera", true);
        //     //SoundPlayer.PlaySound(cameraSound, gameObject);
        // }

      //  else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("CardReward") )
      //  {
      //      // anim.SetBool("MapCamera", false);
      //      if (hasSetCamera)
      //      {
      //          CardReward();
//
      //      }
      //      // anim.SetBool("Up", true);
      //      //SoundPlayer.PlaySound(cameraSound, gameObject);
      //  }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("RuneReward"))
        {

           // if (!hasSetCamera)
           // {
           //     SetCameraView(CameraView.RuneBeforePick);
           //     hasSetCamera = true;
           //     if (lerptime <= 1)
           //     {
           //         hasSetCamera = false;
           //     }
           // }

            //anim.SetBool("MapCamera", false);
            //anim.SetBool("Up", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("DeckLibrary"))
        {
            // anim.SetBool("MapCamera", false);
            // anim.SetBool("Up", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }

        else if (MultiSceneLoader.getLoadedCollectionTitle.Equals("HowToPlay"))
        {
            //anim.SetBool("Up", false);
            //anim.SetBool("HowToPlay", true);
            //SoundPlayer.PlaySound(cameraSound, gameObject);
        }
        else
        {

            // anim.SetBool("HowToPlay", false);
            // anim.SetBool("MapCamera", false);

        }
    }


    public void SetCameraView(CameraView view)
    {
        previusPos = target;
        lerptime = 0;
        switch (view)
        {
            case CameraView.Reset:
            if(hasSetCamera)
                target = _CamPossies[0];
                Debug.Log("Camera is MainView");
                break;

            case CameraView.Right:
            if(hasSetCamera)
                target = _CamPossies[2];
                Debug.Log("Camera is Right");
                break;

            case CameraView.Left:
            if(hasSetCamera)
                target = _CamPossies[1];
                Debug.Log("Camera is Left");

                break;
            case CameraView.Down:
            if(hasSetCamera)
                target = _CamPossies[3];
                Debug.Log("Camera is Down");
                break;

            case CameraView.Up:
            
                break;
            case CameraView.EnemyCloseUp:

                break;
            case CameraView.CardCloseUp:

                break;
            case CameraView.Map:
            if(hasSetCamera)
                target = _CamPossies[4];
                Debug.Log("Camera is Map");
                break;

            case CameraView.RuneBeforePick:
            if(hasSetCamera)
                target = _CamPossies[7];
                Debug.Log("Camera is Rune Before Pick");
                break;

            case CameraView.RuneAfterPick:
            if(hasSetCamera)
                target = _CamPossies[8];
                Debug.Log("Camera is Rune After Pick");
                break;

            case CameraView.CardReward:
            if(hasSetCamera)
                target = _CamPossies[5];
                Debug.Log("Camera is Card Reward");
                break;

            case CameraView.RestPlace:
            if(hasSetCamera)
                target = _CamPossies[6];
                Debug.Log("Camera is Resplace");
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
    Map,
    RuneBeforePick,
    RuneAfterPick,
    RestPlace,
    CardReward

}

[System.Serializable]
public struct CamPos
{
    public Vector3 cameraPos;
    public Quaternion cameraRot;
    public string nameOfPos;
    public CameraView cv;
}
*/