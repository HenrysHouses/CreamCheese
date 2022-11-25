//modified by Charlie

using System.Collections.Generic;
using UnityEngine;
using HH.MultiSceneTools;
using System;

public class ChooseRuneReward : MonoBehaviour
{
    [SerializeField] PlayerTracker _player;
    
    [SerializeField] List<RuneType> runeOptions = new List<RuneType>();

    public Transform[] spots;
    rune[] CardOptions;
    public GameObject prefab;

    [SerializeField]
    LayerMask laneLayer;

    void Awake()
    {
        CardOptions = new rune[spots.Length];
        getRandomRunes();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rune SelectedRune = SelectReward();
            
            if(SelectedRune != null)
            {
                Debug.Log(SelectedRune.RuneData.Name);
                _player.addRune(SelectedRune);

                MultiSceneLoader.loadCollection("Map", collectionLoadMode.Difference);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void getRandomRunes()
    {
        string[] _enums = Enum.GetNames(typeof(RuneType));

        List<string> Randomized = new List<string>();
        foreach (var _enum in _enums)
            Randomized.Add(_enum);

        runeOptions.Clear();

        foreach (var tran in spots)
        {
            int n = UnityEngine.Random.Range(0, Randomized.Count);
            RuneType randomRune = (RuneType) Enum.Parse(typeof(RuneType), Randomized[n]);
            Randomized.RemoveAt(n);
            runeOptions.Add(randomRune);
        }
        InstantiateRune();
    }

    void InstantiateRune()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            if (runeOptions.Count <= 0)
            {
                break;
            }

            GameObject spawn = Instantiate(prefab, spots[i]);
            spawn.transform.localPosition = Vector3.zero;
            spawn.transform.rotation = Quaternion.identity;
            spawn.GetComponent<RuneSelector>().set(runeOptions[i]);
        }
    }

    rune SelectReward()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100, laneLayer))
        {
            for (int i = 0; i < spots.Length; i++)
            {
                Transform target = hit.collider.transform.parent;
                if (target.Equals(spots[i]))
                {
                    return hit.collider.GetComponent<RuneSelector>().Rune;
                }
            }
        }
        Debug.LogError("Could not select rune");
        return null;
    }
}
