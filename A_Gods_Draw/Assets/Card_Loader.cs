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

    // Start is called before the first frame update
    void Start()
    {

        if (card_so as God_Card)
        {
            God_Card test = card_so as God_Card;
            health.text = test.health.ToString();
            image.transform.position -= image.transform.position;
        }
        cardname.text = card_so.cardname;
        image.sprite = card_so.image;
        desc.text = card_so.description;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, 0.5f);
        //transform.Rotate(Vector3.left, 0.5f);
    }
}
