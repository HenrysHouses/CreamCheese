// *
// * Modified by Henrik
// * Modified By Snackolay, sorry
// *
// *
/*
using UnityEngine;
using FMODUnity;
using HH.MultiSceneTools;

public class CameraMovementNonAnims : MonoBehaviour
{

    [SerializeField]
    CamPos[] _CamPossies;
    public static CameraMovementNonAnims instance;
    //  [SerializeField] EventReference MainCave_AMBX;
    // private TurnManager TM;
    private bool looking,hasSetCamera;
    private GameObject battlemusicCheck;
    public GameObject menuMusicCheck, battleMusicG;
    [SerializeField] EventReference cameraSound;
    private CamPos endPosition, startPosition;
    float lerptime;

    // Start is called before the first frame update
    void Start()
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

        bool goDown = Input.GetKeyDown(KeyCode.S);
        bool goRight = Input.GetKeyDown(KeyCode.D);
        bool goUp = Input.GetKeyDown(KeyCode.W);
        bool goLeft = Input.GetKeyDown(KeyCode.A);
        // bool goMap = MultiSceneLoader.getLoadedCollectionTitle.Equals("Map");
        // bool GoCardReward = MultiSceneLoader.getLoadedCollectionTitle.Equals("CardReward");
        // bool GoRuneReward = MultiSceneLoader.getLoadedCollectionTitle.Equals("RuneReward");
        // bool GoLibrary = MultiSceneLoader.getLoadedCollectionTitle.Equals("DeckLibrary");
        // bool GoTutorial = MultiSceneLoader.getLoadedCollectionTitle.Equals("HowToPlay");
        // bool resetView = goLeft && Input.GetKeyDown(KeyCode.D) || goRight && Input.GetKeyDown(KeyCode.A);
       
        if (goDown)
            SetCameraView(CameraView.Down);

        if (goLeft)
            SetCameraView(CameraView.Left);

        if (goRight)
            SetCameraView(CameraView.Right);

        if (goUp)
            SetCameraView(CameraView.Reset);

        if(endPosition.cameraPos != transform.position)
        {
            lerptime += Time.deltaTime * 2.5f;

            transform.position = Vector3.Lerp(startPosition.cameraPos, endPosition.cameraPos, lerptime);
            transform.rotation = Quaternion.Lerp(startPosition.cameraRot, endPosition.cameraRot, lerptime);
        }
    }

    private void SetStartPosition()
    {
        startPosition = new CamPos(transform.position, transform.rotation);
    }

    public void SetCameraView(CameraView view)
    {
        SetStartPosition();
        lerptime = 0;

        for (int i = 0; i < _CamPossies.Length; i++)
        {
            if(_CamPossies[i].cv == view)
            {
                endPosition = _CamPossies[i];
                Debug.Log("Camera is " + endPosition.nameOfPos);
            }
        }
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
    CardReward
}

[System.Serializable]
public struct CamPos
{
    public Vector3 cameraPos;
    public Quaternion cameraRot;
    public string nameOfPos;
    public CameraView cv;

    public CamPos(Vector3 Position, Quaternion Rotation)
    {
        cameraPos = Position;
        cameraRot = Rotation;
        nameOfPos = "Start";
        cv = CameraView.None;
    }
}
*/