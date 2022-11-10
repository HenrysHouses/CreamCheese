using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

[CreateAssetMenu(menuName = "ScriptableObjects/God card"), System.Serializable]
public class God_Card_SO : Card_SO
{
    // [HideInInspector]
    public int strengh;
    public int health;
    public GodActionEnum godAction;

    public GameObject God_Model;

    public EventReference enterBattlefield_SFX;


    private void OnValidate() {
        type = CardType.God;
    }

    public GodDialogue[] dialogues;
}

