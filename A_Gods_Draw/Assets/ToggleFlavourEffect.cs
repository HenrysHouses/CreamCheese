using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleFlavourEffect : MonoBehaviour
{
    [SerializeField]
    Text description;

    Card_SO current;

    bool showingFlavourText = true;

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (!current)
                current = GetComponentInChildren<Card_Loader>().GetCardSO;

            if (showingFlavourText)
            {
                description.text = current.effect;
            }
            else
            {
                description.text = current.description;
            }
            showingFlavourText = !showingFlavourText;
        }
    }
}
