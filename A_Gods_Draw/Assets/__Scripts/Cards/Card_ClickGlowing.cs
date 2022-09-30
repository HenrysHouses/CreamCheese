//charlie

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Card_ClickGlowing : MonoBehaviour
{
    public GameObject[] glowBorders;
    public Transform parent;
    bool isCreated;
    public static Component currentlySelected;

    public enum CardType 
    {
        Attack,
        Defence,
        Buff
    }

    CardType currType;

    /// <summary>
    /// holding over a card makes the card glow in the respective color of the type
    /// </summary>
    private void OnMouseOver()
    {
        //glow borders
        if (!isCreated)
        {
            currentlySelected = this;
            switch (currType)
            {
                case CardType.Attack:
                    AttackCardBorder();
                    break;
                case CardType.Defence:
                    DefenceCardBorder();
                    break;
                case CardType.Buff:
                    BuffCardBorder();
                    break;
            }

        }
    }

    public void setBorder(CardType type)
    {
        currType = type;
    }

    public void AttackCardBorder()
    {
        glowBorders[0].SetActive(true); 
        isCreated = true;
    }
    public void BuffCardBorder()
    {
        glowBorders[1].SetActive(true);
        isCreated = true;
    }
    public void DefenceCardBorder()
    {
        glowBorders[2].SetActive(true);
        isCreated = true;
    }

    /// <summary>
    /// when the mouse is no longer over the card it no longer glows
    /// </summary>
    private void OnMouseExit()
    {
        if (isCreated)
        {
            glowBorders[0].SetActive(false);
            glowBorders[1].SetActive(false);
            glowBorders[2].SetActive(false);
            glowBorders[3].SetActive(false);
            isCreated = false;
        }
    }
}
