using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    int maxHealth = 10;
    [SerializeField]
    int health;
    [SerializeField]
    TMP_Text healthTxt;

    int defendedFor = 0;

    Defense_Behaviour defender;
    TurnManager manager;

    [SerializeField]
    Image arrowImage;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthTxt.text = "HP: " + health.ToString();
    }

    public int GetHealth() { return health; }

    public void CanBeDefended(Defense_Behaviour beh)
    {
        defender = beh;
    }

    public void Defend(int value)
    {
        defendedFor += value;
    }

    public void DealDamage(int amount)
    {
        if (amount > defendedFor)
        {
            health = health - (amount - defendedFor);
            defendedFor = 0;
        }
        else
        {
            defendedFor -= amount;
        }

        if (health <= 0)
        {
            health = 0;
            
        }
        healthTxt.text = "HP: " + health.ToString();
    }

    private void OnMouseDown()
    {
        if (defender)
        {
            defender.ItDefends(this);
            PlayerHideArrow();
            //Debug.Log(defender + " is going to defend this");
        }
        defender = null;
    }

    public void OnNewTurn()
    {
        defendedFor = 0;
    }

    public void PlayerShowArrow()
    {
        arrowImage.enabled = true;
    }

    public void PlayerHideArrow()
    {
        arrowImage.enabled = false;
    }
}
