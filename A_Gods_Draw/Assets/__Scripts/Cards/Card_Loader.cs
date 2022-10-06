using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Loader : MonoBehaviour
{
    Card_SO card_so;

    [SerializeField]
    Image image;
    [SerializeField]
    Text cardname;
    [SerializeField]
    Text desc;
    [SerializeField]
    Text health;
    [SerializeField]
    Image typeIcon;
    [SerializeField]
    Text strengh;

    private void Start()
    {
        GetComponent<Canvas>().worldCamera = Camera.main;
    }

    // Start is called before the first frame update
    public void Set(Card_SO card, TurnManager manager)
    {

        card_so = card;
        var godCard = card_so as God_Card;
        if (godCard)
        {
            typeIcon.enabled = false;
            strengh.enabled = false;
            God_Card test = godCard;
            health.text = test.health.ToString();
            image.transform.localPosition -= Vector3.up * image.transform.localPosition.y;
            image.transform.localScale = new Vector3(image.transform.localScale.x, image.transform.localScale.y * 2, image.transform.localScale.z);
        }
        else if (card_so as NonGod_Card)
        {
            health.enabled = false;
            NonGod_Card test = card_so as NonGod_Card;
            typeIcon.sprite = test.icon;
            strengh.text = test.baseStrengh.ToString();
        }
        else
        {
            //Debug.LogError("wtfff don't use the Card_SO objects i might cry");
        }
        cardname.text = card_so.cardname;
        image.sprite = card_so.image;
        desc.text = card_so.description;

        card_so.Init(this.gameObject).SetManager(manager);
    }

    public void moveCardToHand()
    {
        SendMessageUpwards("AddCard", card_so);
        // Debug.Log("Message sent from " + this.gameObject.GetComponent<Card_Behaviour>());
    }

    public void ChangeStrengh(int newValue)
    {
        strengh.text = newValue.ToString();
    }
}
