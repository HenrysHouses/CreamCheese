//charlie

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card_InspectingPopup : MonoBehaviour
{
    public TMP_Text[] descriptions;

    public void SetDescriptions(ActionCard_ScriptableObject card, GameObject targetObject)
    {
        string ability = ActionCard_ScriptableObject.getEffectFormatted(card);
        descriptions[2].text = ability;

        descriptions[0].text = card.description; //lore
        string unlocks = targetObject.GetComponentInChildren<LevelController>().setDescriptionShowAllLevels();
        descriptions[1].text = unlocks; //unlocks
    }


}
