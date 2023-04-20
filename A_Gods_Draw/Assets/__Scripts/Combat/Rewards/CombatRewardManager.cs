using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map;

public class CombatRewardManager : MonoBehaviour
{
    [SerializeField] LootPoolTypes[] LootPools;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[System.Serializable]
public struct LootPoolTypes
{
    public NodeType RewardType;
    public LootPool_ScriptableObject LootPool;
}
