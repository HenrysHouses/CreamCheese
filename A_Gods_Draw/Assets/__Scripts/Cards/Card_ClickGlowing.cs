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
    bool isCreated, hasArrow;
    public static Component currentlySelected;

    TurnManager turnManager;

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
        if(turnManager == null)
        {
            turnManager = GetComponentInChildren<Card_Behaviour>().GetManager();
        }

        //glow borders
        if (!isCreated)
        {
            currentlySelected = this;
            switch (currType)
            {
                case CardType.Attack:
                    AttackCardBorder();
                    if (!hasArrow)
                    {
                        DrawArrowsEnemies();
                    }
                    break;
                case CardType.Defence:
                    DefenceCardBorder();
                    if (!hasArrow)
                    {
                        DrawArrowsPlayer();
                    }
                    break;
                case CardType.Buff:
                    BuffCardBorder();
                    if (!hasArrow)
                    {
                        DrawArrowsCards();
                    }
                    break;
            }
        }
    }

    public void setBorder(CardType type)
    {
        currType = type;
    }

    //when you have an attack card draw arrow over enemies
    public void DrawArrowsEnemies()
    {
        foreach(IMonster monster in turnManager.GetCurrentBoard().enemies)
        {
            monster.ShowArrow();
        }
        hasArrow = true;
    }

    //when you have a defence card draw arrow over player health and God card
    public void DrawArrowsPlayer()
    {
        hasArrow = true;
    }

    //when you have a buff card draw arrow over the place the card will be
    public void DrawArrowsCards()
    {
        hasArrow = true;
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
            for(int i = 0; i < 4; i++)
            {
                glowBorders[i].SetActive(false);
            }
            isCreated = false;
        }

        if (hasArrow)
        {
            //enemies
            foreach (IMonster monster in turnManager.GetCurrentBoard().enemies)
            {
                monster.HideArrow();
                hasArrow = false;
            }

            //player

            //card
        }
    }
}
