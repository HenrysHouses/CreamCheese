// Written by Javier Villegas
// Edited by Henrik

using UnityEngine;
using System.Collections;
using TMPro;
using FMODUnity;

/// <summary>
/// Enum for all the possible actions a card can do
/// </summary>
[System.Serializable]
public enum CardActionEnum
{
    Attack,
    Defence,
    Buff,
    Chaos,
    Chained,
    Offering,
    SplashDMG,
    Heal,
    HealPrevention,
    Poison,
    Draw,
    Weaken,
    Frostbite,
    Leach,
    Exhaust,
    BuffAll,
    Earthquake

}

/// <summary>
/// Enum for each of the possible god actions
/// </summary>
[System.Serializable]
public enum GodActionEnum
{
    None,
    Tyr,
    Eir
}

/// <summary>
/// Struct containing the elements of the prefab
/// </summary>
[System.Serializable]
public struct CardElements
{
    public Renderer ArtRenderer;
    public Renderer OrmRenderer;
    public Renderer CardRenderer;
    public Renderer LevelRenderer;
    public GameObject Glow;
    public TextMeshPro cardName;
    public TextMeshPro desc;
    public TextMeshPro strength;
    public EventReference OnClickSFX;
    public UIPopup Description;
    // public UIPopup Effect;
    public LevelController level;
    public GameObject LevelUpVFX;
}


/// <summary>
/// Class that given a Card_SO, modifies the elements on it to represent which card is and,
/// if necessary, adds the corresponding behaviour
/// </summary>
public class Card_Loader : MonoBehaviour
{
    [SerializeField] Texture transparentMetallic;
    [field:SerializeField] public CardElements elements {get; private set;} 
    [SerializeField] bool LoadCardPlayData = false;
    public CardPlayData _card;
    Card_Behaviour CB;
    public bool isDissolving {private set; get;}

    
    public bool addComponentAutomatically = true;

    public Card_SO GetCardSO => _card.CardType;
    public Card_Behaviour Behaviour => CB;

//------------------------------------------

    private void Start()
    {
        SetDissolve(0);
        
        if(_card.CardType != null && LoadCardPlayData)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3(-0.00250761397f,0.000386005268f,0.000921862083f);
            collider.size = new Vector3(0.140063912f,0.198602021f,0.00708327722f);
            Set(_card);
        }
    }
    void ChangeOrm(CardType card)
    {
        switch(card)
        {
            case CardType.Attack:
                elements.OrmRenderer.material.color = Color.red;
                break;
            case CardType.Defence:
                elements.OrmRenderer.material.color = Color.blue;
                break;
            case CardType.Buff:
                elements.OrmRenderer.material.color = Color.blue + Color.red;
                break;
            case CardType.Utility:
                elements.OrmRenderer.material.color = Color.yellow;
                break;
        }
    }

    public IEnumerator DissolveCard(float speed = 1, Transform Parent = null)
    {
        if(isDissolving)
            yield break;

        float t = 0;
        isDissolving = true;

        while(t <= 1)
        {
           t += Time.deltaTime * speed;
            SetDissolve(t);
            yield return new WaitForEndOfFrame();
        }

        if(Parent)
            Parent.position  += Vector3.down * 10;
        else
            transform.position += Vector3.down * 10;
        isDissolving = false;
    }

    public void SetDissolve(float amount)
    {
        elements.ArtRenderer.material.SetFloat("_Cutoff",amount);
        elements.OrmRenderer.material.SetFloat("_Cutoff",amount);
        elements.CardRenderer.material.SetFloat("_Cutoff",amount);

        if(amount == 1)
        {
            elements.ArtRenderer.material.SetFloat("_UseShadows",0);
            elements.OrmRenderer.material.SetFloat("_UseShadows",0);
            elements.CardRenderer.material.SetFloat("_UseShadows",0);
        }
    }

    public void instantiateLvLUpVFX(int fromLvl, int toLvl)
    {
        GameObject VFX = Instantiate(elements.LevelUpVFX);
        LvlUp_VFX _controller = VFX.GetComponent<LvlUp_VFX>();
        _controller.CurrentLevel = fromLvl;
        _controller.NextLevel = toLvl;
        VFX.transform.SetParent(transform, false);
    }

    //------------------------------------------

    /// <summary> The method that modifies the card gameobject </summary>
    /// <param name="card">Data required to build the prefab and get its Levels and current Up</param>
    public void Set(CardPlayData card)
    {
        bool isReset = _card.CardType != null;
        bool UseUpgradedDescription = false;
        _card = card;

        elements.cardName.text = _card.CardType.cardName;
        elements.cardName.ForceMeshUpdate();

        // if(elements.level != null && card.CardType is ActionCard_ScriptableObject)
        // {
            elements.level.set(GetComponent<Card_Selector>(), card, !addComponentAutomatically);
        // }

        if(elements.Description != null)
        {
            Popup_ScriptableObject copy = ScriptableObject.CreateInstance<Popup_ScriptableObject>();
            elements.Description.PopupInfo.Clone(ref copy);
            elements.Description.PopupInfo = copy;
        }

       // if(elements.Description != null)
       // {
       //     Popup_ScriptableObject copy = ScriptableObject.CreateInstance<Popup_ScriptableObject>();
       //     elements.Effect.PopupInfo.Clone(ref copy);
       //     copy.Info = card.CardType.effect;
       //     elements.Effect.PopupInfo = copy;
       // }


        if(_card.CardType.Background)
            elements.ArtRenderer.material.SetTexture("_MainTex", _card.CardType.Background);
        if(_card.CardType.Art)
            elements.ArtRenderer.material.SetTexture("_ArtTex", _card.CardType.Art);

        if (_card.CardType is GodCard_ScriptableObject)
        {
            GodCard_ScriptableObject god_card = _card.CardType as GodCard_ScriptableObject;
            elements.strength.text = god_card.health.ToString();
            elements.Glow.SetActive(true);
            //  Gold color
            elements.OrmRenderer.material.color = new Color(1, 0.6458119f, 0);
            elements.OrmRenderer.material.SetTexture("_MetallicGlossMap", transparentMetallic);
            elements.OrmRenderer.material.SetFloat("_GlossMapScale", 0.75f);
            elements.CardRenderer.material.color = new Color(1, 0.6458119f, 0);
            elements.CardRenderer.material.SetTexture("_MetallicGlossMap", transparentMetallic);
            elements.CardRenderer.material.SetFloat("_GlossMapScale", 0.75f);
            
            if (addComponentAutomatically)
            {
                CB = gameObject.AddComponent<GodCard_Behaviour>();
                (CB as GodCard_Behaviour).Initialize(god_card, elements);
                (CB as GodCard_Behaviour).ApplyLevels(card.Experience);

                elements.LevelRenderer.gameObject.SetActive(false);
            }
        }
        else
        {
            elements.Glow.SetActive(false);
            ActionCard_ScriptableObject Action_Card = _card.CardType as ActionCard_ScriptableObject;


            elements.strength.text = Action_Card.cardStats.formattedStrength;

            // border color
            ChangeOrm(Action_Card.type);

            if(addComponentAutomatically)
            {
                CB = gameObject.AddComponent<ActionCard_Behaviour>();
                (CB as ActionCard_Behaviour).Initialize(Action_Card, elements);

                UseUpgradedDescription = (CB as ActionCard_Behaviour).ApplyLevels(card.Experience);
            }
            elements.level.instantiateIcons(Action_Card.cardStats.getGlyphs(Action_Card.type), !addComponentAutomatically, isReset);
        }

        if(elements.Description.PopupInfo)
        {
            ActionCard_ScriptableObject info = card.CardType as ActionCard_ScriptableObject;
            
            if(info && !UseUpgradedDescription)
                elements.Description.PopupInfo.Info = info.getEffectFormatted();
            else if(info)
                elements.Description.PopupInfo.Info = info.getEffectFormatted(CB.stats);
            else
                elements.Description.PopupInfo.Info = card.CardType.effect;
        }

        if(elements.level != null && card.CardType is ActionCard_ScriptableObject _Action_Card)
        {
            CardUpgrade[] upgrades = _Action_Card.cardStats.UpgradePath.Upgrades;


            if(addComponentAutomatically)
            {
                if(!elements.level.UpdateLevelFill(upgrades, card.Experience))
                        elements.LevelRenderer.gameObject.SetActive(false);

                elements.level.setDescriptionToLevel(card.Experience.Level);
            }
            else
            {
                if(!elements.level.UpdateLevelFill(upgrades, card.Experience, true))
                    elements.LevelRenderer.gameObject.SetActive(false);
                
                elements.level.setDescriptionShowAllLevels();
            }
        }
    }
}