using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class God_Behaviour : Card_Behaviour
{
    public new void OnPlay()
    {
        SearchToBuff();
    }

    void SearchToBuff()
    {
        //for each current nongod-card: getgodbuff
    }


}
