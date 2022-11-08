using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

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
}

[System.Serializable]
public enum GodActionEnum
{
    None,
    Tyr,
}

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

public class Card_Loader : MonoBehaviour
{
    [SerializeField] Card_SO card_so;
    public Card_SO GetCardSO => card_so;

    [SerializeField]
    CardElements elements;

    private Card_Behaviour CB;
    
    public bool shouldAddComponent = true;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        elements.prop = transform.GetChild(transform.childCount - 1);
    }

    // Start is called before the first frame update
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
            elements.strength.text = nonGod.cardActions[nonGod.cardStrenghIndex].actionStrength.ToString();

            ChangeOrm(nonGod.type);

            if(shouldAddComponent)
            {
                CB = gameObject.AddComponent<NonGod_Behaviour>();
                (CB as NonGod_Behaviour).Initialize(nonGod, elements);
            }
        }


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

        if (card == CardType.Buff)
        {
            elements.prop.GetChild(5).gameObject.SetActive(true);
            return;
        }
    }

    public Card_Behaviour Behaviour => CB;
}
