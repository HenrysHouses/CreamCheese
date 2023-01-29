// Written by Javier Villegas
// Edited by Henrik

using UnityEngine;
using UnityEngine.UI;
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
    Instakill,
    Chained,
    Storr,
    SplashDMG,
    Heal,
    HealPrevention,
    Poison,
    Draw,
    Weaken,
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
    public TextMeshPro cardName;
    public TextMeshPro desc;
    public TextMeshPro strength;
    public EventReference OnClickSFX;
}


/// <summary>
/// Class that given a Card_SO, modifies the elements on it to represent which card is and,
/// if necessary, adds the corresponding behaviour
/// </summary>
public class Card_Loader : MonoBehaviour
{
    [SerializeField] Texture transparentMetallic;
    [SerializeField] PathController IconPath;
    [SerializeField] GameObject IconPrefab;
    [SerializeField] CardElements elements;
    [SerializeField] Card_SO card_so;
    Card_Behaviour CB;

    // [HideInInspector]
    public bool addComponentAutomatically = true;

    public Card_SO GetCardSO => card_so;
    public Card_Behaviour Behaviour => CB;

//------------------------------------------

    private void Start()
    {
        if(card_so)
            Set(card_so);
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

    //------------------------------------------

    /// <summary> The method that modifies the card gameobject </summary>
    /// <param name="card"> The Card_SO object to get the data from </param>
    public void Set(Card_SO card)
    {
        card_so = card;

        elements.cardName.text = card_so.cardName;
        elements.cardName.ForceMeshUpdate();
        elements.ArtRenderer.material.SetTexture("_MainTex", card_so.Background);
        elements.ArtRenderer.material.SetTexture("_ArtTex", card_so.Art);
        
        // Might remove
        // elements.desc.text = card_so.effect;
        // elements.desc.ForceMeshUpdate();

        if (card_so is God_Card_SO)
        {
            God_Card_SO god_card = card_so as God_Card_SO;
            elements.strength.text = god_card.health.ToString();

            //  Gold color
            elements.OrmRenderer.material.color = new Color(1, 0.6458119f, 0);
            elements.OrmRenderer.material.SetTexture("_MetallicGlossMap", transparentMetallic);
            elements.OrmRenderer.material.SetFloat("_GlossMapScale", 0.75f);
            elements.CardRenderer.material.color = new Color(1, 0.6458119f, 0);
            elements.CardRenderer.material.SetTexture("_MetallicGlossMap", transparentMetallic);
            elements.CardRenderer.material.SetFloat("_GlossMapScale", 0.75f);
            
            if (addComponentAutomatically)
            {
                CB = gameObject.AddComponent<God_Behaviour>();
                (CB as God_Behaviour).Initialize(god_card, elements);
            }
        }
        else
        {
            NonGod_Card_SO nonGod = card_so as NonGod_Card_SO;

            if (nonGod.targetActions.Count > 0)
                elements.strength.text = nonGod.targetActions[0][nonGod.cardStrenghIndex].actionStrength.ToString();

            // border color
            ChangeOrm(nonGod.type);

            if(addComponentAutomatically)
            {
                CB = gameObject.AddComponent<NonGod_Behaviour>();
                (CB as NonGod_Behaviour).Initialize(nonGod, elements);
            }
        }

        instantiateIcons();
    }

    private void instantiateIcons()
    {
        float pos = 1/(card_so.Icons.Count+1f);

        for (int i = 0; i < card_so.Icons.Count; i++)
        {

            GameObject icon = Instantiate(IconPrefab);
            icon.transform.SetParent(IconPath.transform.parent);

            OrientedPoint OP = IconPath.GetEvenPathOP(pos * (i+1));
            icon.transform.position = OP.pos;

            icon.GetComponent<IconController>().setIcon(card_so.Icons[i]);
        }
    }
}