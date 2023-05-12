// *
// * Written By Henrik
// *
// * modified by Charlie
// *

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
using System;

public class ChooseRuneReward : MonoBehaviour
{
    RuneStoneController RuneController;
    [SerializeField] CardReaderController Inspector;
    [SerializeField] PlayerTracker _player;
    [SerializeField] PathController Path;
    [SerializeField] float animSpeed = 0.5f;
    float RuneAnimationT;
    public Transform[] spots;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;
    bool confirmed, isClicked;
    public bool shouldConfirmSelection;

    void Awake()
    {
        RuneController = GameObject.FindObjectOfType<RuneStoneController>();
    }

    void Start()
    {
        CameraMovement.instance.SetCameraView(CameraView.RuneBeforePick);
        RuneAnimationT = 0;
    }

    private void Update()
    {
        if(RuneAnimationT <= 0)
            findRune();
    }

    // bool hasClicked = false;
    void findRune()
    {
        if(GameManager.instance.PauseMenuIsOpen)
        {
            return;
        }

        if (!confirmed)
            checkSelected();

        if(Inspector.isInspecting)
        {
            if(Input.GetMouseButtonDown(1))
            {
                Inspector.returnInspection();
            }
            
            if (Input.GetMouseButtonDown(0)) // Confirm
            {
                if(!Inspector.isInspecting)
                {
                    Debug.Log("return");
                    Inspector.returnInspection();
                    return;
                }
            


            }

            // if(Input.GetMouseButtonDown(0))
            // {
            //     hasClicked = true;
            // }
        }
        // else
        //     hasClicked = false;
    }

    void checkSelected()
    {
        rune SelectedRune = SelectReward(out GameObject runeObj);

        // Debug.Log(SelectedRune + " / " + runeObj);

        if (shouldConfirmSelection)
        {
            Debug.Log("configrmsdksadnk");
            StartCoroutine(PickRune(SelectedRune, runeObj));
        }
        shouldConfirmSelection = false;
    }

    IEnumerator PickRune(rune SelectedRune, GameObject obj)
    {
        if(SelectedRune.GetType() != typeof(rune))
        {
            Inspector.enabled = false;
            Path.startPoint.position = obj.transform.position;
            Path.endPoint.position = RuneController.renderers[(int)SelectedRune.RuneData.Name].renderers[0].transform.parent.position;
            Path.recalculatePath();

            CameraMovement.instance.SetCameraView(CameraView.RuneAfterPick);
            
            RuneAnimationT = 0;

            while(RuneAnimationT < 1)
            {
                OrientedPoint OP = Path.GetEvenPathOP(RuneAnimationT);
                obj.transform.position = OP.pos;
                RuneAnimationT += Time.deltaTime * animSpeed;
                yield return new WaitForEndOfFrame();
            }

            _player.addRune(SelectedRune);
            Destroy(obj);
            Map.Map_Manager.SavingMap();
            yield return new WaitForSeconds(1);
            CameraMovement.instance.SetCameraView(CameraView.Up);
            MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
        }
    }

    rune SelectReward(out GameObject obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, laneLayer))
        {
            for (int i = 0; i < spots.Length; i++)
            {
                Transform target = hit.collider.transform.parent;

                if (target.Equals(spots[i]))
                {
                    hit.transform.GetChild(0).gameObject.SetActive(true);
                    obj = hit.collider.gameObject;
                    return hit.collider.GetComponent<RuneSelector>().Rune;
                }
            }
        }
        obj = null;
        return null;
    }

    // public void getRandomRunes()
    // {
    //     string[] _enums = Enum.GetNames(typeof(RuneType));

    //     List<string> Randomized = new List<string>();
    //     foreach (var _enum in _enums)
    //         Randomized.Add(_enum);

    //     runeOptions.Clear();

    //     foreach (var tran in spots)
    //     {
    //         int n = UnityEngine.Random.Range(0, Randomized.Count);
    //         RuneType randomRune = (RuneType) Enum.Parse(typeof(RuneType), Randomized[n]);
    //         Randomized.RemoveAt(n);
    //         runeOptions.Add(randomRune);
    //     }
    //     InstantiateRune();
    // }

    // void InstantiateRune()
    // {
    //     for (int i = 0; i < spots.Length; i++)
    //     {
    //         if (runeOptions.Count <= 0)
    //         {
    //             break;
    //         }

    //         GameObject spawn = Instantiate(prefab, spots[i]);
    //         spawn.transform.localPosition = Vector3.zero;
    //         spawn.transform.rotation = Quaternion.identity;
    //         spawn.GetComponent<RuneSelector>().set(runeOptions[i]);
    //     }
    // }

}