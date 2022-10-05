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
        Buff,
        God
    }

    CardType currType;

    /// <summary>
    /// 1.holding over a card makes the card glow in the respective color of the type
    /// 2.makes an arrow show where to be placed
    /// </summary>
    private void OnMouseOver()
    {
        if (turnManager == null && gameObject.transform.childCount != 0)
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
                case CardType.God:
                    GodCardBorder();
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
        if (turnManager != null)
        {
            foreach (IMonster monster in turnManager.GetCurrentBoard().enemies)
            {
                monster.EnemyShowArrow();
            }
            hasArrow = true;
        }
    }

    //when you have a defence card draw arrow over player health and God card
    public void DrawArrowsPlayer()
    {
        if (turnManager.GetCurrentBoard().player)
        {
            PlayerController player = turnManager.GetCurrentBoard().player;
            player.PlayerShowArrow();
        }
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

    public void GodCardBorder()
    {
        glowBorders[3].SetActive(true);
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
            if (turnManager != null)
            {
                //enemies
                foreach (IMonster monster in turnManager.GetCurrentBoard().enemies)
                {
                    monster.EnemyHideArrow();
                    hasArrow = false;
                }

                //player
                if (turnManager.GetCurrentBoard().player)
                {
                    PlayerController player = turnManager.GetCurrentBoard().player;
                    player.PlayerHideArrow();
                    hasArrow = false;
                }

                //buff cards

            }
        }
    }
}
