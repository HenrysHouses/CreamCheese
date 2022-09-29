using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthTxt.text = "HP: " + health.ToString();
    }

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
            Debug.Log("You Died lmao noob");
        }
        healthTxt.text = "HP: " + health.ToString();
    }

    private void OnMouseDown()
    {
        if (defender)
        {
            defender.ItDefends(this);
            //Debug.Log(defender + " is going to defend this");
        }
        defender = null;
    }
}
