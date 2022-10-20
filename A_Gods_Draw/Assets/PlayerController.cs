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
    // TurnManager manager;

    [SerializeField]
    Image arrowImage;
    [SerializeField]
    GameObject shieldObject;

    TMP_Text shieldText;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthTxt.text = "HP: " + health.ToString();
        shieldText = shieldObject.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
    }

    public int GetHealth() { return health; }

    public void CanBeDefended(Defense_Behaviour beh)
    {
        defender = beh;
    }

    public void Defend(int value)
    {
        if (defendedFor == 0)
        {
            shieldObject.SetActive(true);
        }
        defendedFor += value;
        shieldText.text = defendedFor.ToString();
    }

    public void DealDamage(int amount)
    {
        if (amount > defendedFor)
        {
            health = health - (amount - defendedFor);
            defendedFor = 0;
            shieldText.text = defendedFor.ToString();
            shieldObject.SetActive(false);
        }
        else
        {
            defendedFor -= amount;
            shieldText.text = defendedFor.ToString();
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
            shieldObject.SetActive(true); //needs to be able to change the value of how much its shielding for
            defender.ItDefends(this);
            PlayerHideArrow();
            //Debug.Log(defender + " is going to defend this");
        }
        defender = null;
    }

    public void OnNewTurn()
    {
        defendedFor = 0;
        shieldText.text = defendedFor.ToString();
        shieldObject.SetActive(false);
    }

    private void OnMouseOver()
    {
        arrowImage.color = Color.blue;
    }

    private void OnMouseExit()
    {
        arrowImage.color = Color.white;
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
