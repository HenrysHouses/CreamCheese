using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardConfirmation : MonoBehaviour
{
    public ChooseCardReward chooseCardReward;
    public ChooseRuneReward chooseRuneReward;

    private void OnMouseDown()
    {
        if(chooseCardReward)
        {
            chooseCardReward.shouldConfirmSelection = true;
            Debug.Log("confirm card");
        }

        if(chooseRuneReward)
        {
            chooseRuneReward.shouldConfirmSelection = true;
            Debug.Log("confirm card");
        }
    }
}