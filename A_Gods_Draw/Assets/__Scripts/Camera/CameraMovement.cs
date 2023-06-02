// *
// * Modified by Henrik
// * Modified By Snackolay, sorry
// *
// *

using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools;

public class CameraMovement : MonoBehaviour
{

    public CameraView cameraViewContainer;
    [SerializeField] CamPos[] _CamPossies;
    [SerializeField] CamPos TemporaryPosition;
    public static CameraMovement instance;
    //  [SerializeField] EventReference MainCave_AMBX;
    // private TurnManager TM;
    private bool looking,hasSetCamera;
    private GameObject battlemusicCheck;
    public GameObject menuMusicCheck, battleMusicG;
    [SerializeField] EventReference cameraSound;
    private CamPos endPosition, startPosition;
    float lerptime;
    bool isAtLocation;
    public bool lockInputs;


    void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;

        endPosition = GetCamPos(CameraView.Reset);
        startPosition = GetCamPos(CameraView.Reset);
    }

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

        bool goDown = false;
        bool goRight = false;
        bool goUp = false;
        bool goLeft = false;

        if(!lockInputs)
        {
            goDown = Input.GetKeyDown(KeyCode.S);
            goRight = Input.GetKeyDown(KeyCode.D);
            goUp = Input.GetKeyDown(KeyCode.W);
            goLeft = Input.GetKeyDown(KeyCode.A);
        }
        
        if (goDown && cameraViewContainer != CameraView.Reset)
            SetCameraView(CameraView.Reset);
        else if(goDown)
        {
            SetCameraView(CameraView.Down);
        }

        if (goLeft)
            SetCameraView(CameraView.Left);

        if (goRight)
            SetCameraView(CameraView.Right);

        if (goUp && cameraViewContainer != CameraView.Reset)
            SetCameraView(CameraView.Reset);

        else if(goUp)
        {
            SetCameraView(CameraView.Up);
            
        }

        if(endPosition.transform.position != transform.position)
        {
            lerptime += Time.deltaTime * 2.5f;

            transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, lerptime);
            transform.rotation = Quaternion.Lerp(startPosition.transform.rotation, endPosition.transform.rotation, lerptime);
        }
    }

    private void SetStartPosition()
    {
        startPosition = TemporaryPosition;
        TemporaryPosition.transform.position = transform.position;
        TemporaryPosition.transform.rotation = transform.rotation;
    }

    public void SetCameraView(CameraView view, bool LockView = false)
    {
        SetStartPosition();
        lerptime = 0;
        cameraViewContainer = view;

        for (int i = 0; i < _CamPossies.Length; i++)
        {
            if(_CamPossies[i].cv == view)
            {
                endPosition = _CamPossies[i];
                // Debug.Log("Camera is " + endPosition.cv);
            }
        }

        if(view == CameraView.Reset)
            lockInputs = false;

        if(LockView)
            lockInputs = true;
    }

    public CamPos GetCamPos(CameraView view)
    {
        for (int i = 0; i < _CamPossies.Length; i++)
        {
            if(_CamPossies[i].cv == view)
            {
                return _CamPossies[i];
            }
        }
        throw new UnityException("Camera position was not defined: " + view);
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
    CardReward,
    Library,
    BoardTopView,
    TakingDamage,
    Map_EarlyEncounter,
    Map_MidEncounter,
    Map_LateEncounter,
    DestroyCard
}

[System.Serializable]
public struct CamPos
{
    public Transform transform;
    public CameraView cv;

    public CamPos(Transform transform)
    {
        this.transform = transform;
        cv = CameraView.None;
    }
}
