// Written by Javier Villegas

using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

/// <summary>
/// Enum for all the possible actions a card can do
/// </summary>
[System.Serializable]
public enum CardActionEnum
{
    Attack,
    Defend,
    Buff,
    Instakill,
    Chained,
    Storr,
    SplashDMG,
    Heal,
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
}

/// <summary>
/// Struct containing the elements of the prefab
/// </summary>
[System.Serializable]
public struct CardElements
{
    public Image image;
    public Text cardName;
    public Text desc;
    public Text health;
    public Image typeIcon;
    public Text strength;

    public Transform prop;

    public EventReference OnClickSFX;

}


/// <summary>
/// Class that given a Card_SO, modifies the elements on it to represent which card is and,
/// if necessary, adds the corresponding behaviour
/// </summary>
public class Card_Loader : MonoBehaviour
{
    [SerializeField]
    CardElements elements;
    Card_SO card_so;
    Card_Behaviour CB;

    [HideInInspector]
    public bool shouldAddComponent = true;

    public Card_SO GetCardSO => card_so;
    public Card_Behaviour Behaviour => CB;

//------------------------------------------

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
        elements.prop = transform.GetChild(transform.childCount - 1);
    }
    void ChangeOrm(CardType card)
    {
        if (card == CardType.Attack)
        {
            elements.prop.GetChild(3).gameObject.SetActive(true);
            return;
        }

        if (card == CardType.Defence)
        {
            elements.prop.GetChild(4).gameObject.SetActive(true);
            return;
        }

        else
        {
            elements.prop.GetChild(5).gameObject.SetActive(true);
            return;
        }
    }

    //------------------------------------------

    /// <summary> The method that modifies the card gameobject </summary>
    /// <param name="card"> The Card_SO object to get the data from </param>
    public void Set(Card_SO card)
    {
        card_so = card;

        elements.cardName.text = card_so.cardName;
        elements.image.sprite = card_so.image;
        elements.desc.text = card_so.description;

        if (card_so is God_Card_SO)
        {
            God_Card_SO god_card = card_so as God_Card_SO;
            elements.typeIcon.enabled = false;
            elements.strength.enabled = false;
            elements.health.text = god_card.health.ToString();
            elements.image.transform.localPosition -= Vector3.up * elements.image.transform.localPosition.y;

            elements.prop.GetChild(1).gameObject.SetActive(true);


            if (shouldAddComponent)
            {
                CB = gameObject.AddComponent<God_Behaviour>();
                (CB as God_Behaviour).Initialize(god_card, elements);
            }
        }
        else
        {
            NonGod_Card_SO nonGod = card_so as NonGod_Card_SO;

            //Debug.Log(nonGod);
            elements.health.enabled = false;

            // elements.typeIcon.sprite = nonGod.icon;
            if (nonGod.targetActions.Count > 0)
                elements.strength.text = nonGod.targetActions[0][nonGod.cardStrenghIndex].actionStrength.ToString();

            ChangeOrm(nonGod.type);

            if(shouldAddComponent)
            {
                CB = gameObject.AddComponent<NonGod_Behaviour>();
                (CB as NonGod_Behaviour).Initialize(nonGod, elements);
            }
        }


    }
}
