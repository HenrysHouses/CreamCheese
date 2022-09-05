using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card_Loader : MonoBehaviour
{
    [SerializeField]
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

    // Start is called before the first frame update
    void Start()
    {

        if (card_so as God_Card)
        {
            typeIcon.enabled = false;
            strengh.enabled = false;
            God_Card test = card_so as God_Card;
            health.text = test.health.ToString();
            image.transform.position -= image.transform.position;
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
            Debug.LogError("wtfff don't use the Card_SO objects i might cry");
        }
        cardname.text = card_so.cardname;
        image.sprite = card_so.image;
        desc.text = card_so.description;

        card_so.Init(gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 70 * Time.deltaTime);
        //transform.Rotate(Vector3.left, 0.5f);
    }
}
