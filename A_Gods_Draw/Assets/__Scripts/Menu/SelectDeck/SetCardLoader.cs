using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCardLoader : MonoBehaviour
{
    public Card_Loader loader;

    public void setLoader(Card_SO card)
    {
        CardPlayData playData = new CardPlayData(card);

        loader.Set(playData);
    }
}
