using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardRewardConfirmation : MonoBehaviour
{
    [SerializeField] ChooseCardReward chooseCardReward;

    private void OnMouseDown()
    {
        chooseCardReward.shouldConfirmSelection = true;
    }
}
