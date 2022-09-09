using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[CreateAssetMenu(menuName = "ScriptableObjects/Card_Abs")]

public abstract class Card_SO : ScriptableObject
{
    public Sprite image;
    public string cardname;
    public string description;
    public GameObject cardObject;

    public virtual void Init(GameObject a)
    {
    }

}

