using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Loader : MonoBehaviour
{
    Card_SO card_so;
    public Card_SO GetCardSO => card_so;

    [SerializeField]
    Image image;
    [SerializeField]
    Text cardName;
    [SerializeField]
    Text desc;
    [SerializeField]
    Text health;
    [SerializeField]
    Image typeIcon;
    [SerializeField]
    Text strength;

    [SerializeField]
    Transform prop;
    public bool shouldAddComponent = true;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;

        prop = transform.GetChild(transform.childCount - 1);
    }

    // Start is called before the first frame update
    public void Set(Card_SO card)
    {

        card_so = card;
        var godCard = card_so as God_Card;
        if (godCard)
        {
            typeIcon.enabled = false;
            strength.enabled = false;
            God_Card test = godCard;
            health.text = test.health.ToString();
            image.transform.localPosition -= Vector3.up * image.transform.localPosition.y;
            image.transform.localScale = new Vector3(image.transform.localScale.x, image.transform.localScale.y, image.transform.localScale.z);

            prop.GetChild(1).gameObject.SetActive(true);
        }
        else if (card_so as NonGod_Card)
        {
            NonGod_Card nonGod = card_so as NonGod_Card;
            health.enabled = false;
            typeIcon.sprite = nonGod.icon;
            strength.text = nonGod.baseStrength.ToString();

            ChangeOrm(nonGod);
        }
        else
        {
            //Debug.LogError("wtfff don't use the Card_SO objects i might cry");
        }
        cardName.text = card_so.cardName;
        image.sprite = card_so.image;
        desc.text = card_so.description;

        // if(shouldAddComponent)
        //     card_so.Init(this.gameObject).SetManager(manager); // ! probably
    }

    void ChangeOrm(NonGod_Card card)
    {
        if (card is Attack_Card attack_)
        {
            prop.GetChild(3).gameObject.SetActive(true);
            return;
        }
        
        if (card is Defense_Card defense_)
        {
            prop.GetChild(4).gameObject.SetActive(true);
            return;
        }
        
        // card is Buff_Card
        prop.GetChild(5).gameObject.SetActive(true);
    }

    public void ChangeStrengh(int newValue)
    {
        strength.text = newValue.ToString();
    }
}
