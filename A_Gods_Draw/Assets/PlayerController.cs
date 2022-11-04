using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : BoardElement
{
    [SerializeField] 
    PlayerTracker playerTracker;
    [SerializeField]
    int maxHealth = 10;
    [SerializeField]
    TMP_Text healthTxt;

    int defendedFor = 0;
    // TurnManager manager;

    [SerializeField]
    Image arrowImage;
    [SerializeField]
    GameObject shieldObject;

    TMP_Text shieldText;

    TurnController controller;

    // Start is called before the first frame update
    void Start()
    {
        healthTxt.text = "HP: " + playerTracker.Health.ToString();
        shieldText = shieldObject.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
    }

    // public int GetHealth() { return health; }

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
            int diff = amount - defendedFor;
            Debug.Log("Damage taken:" + -diff);
            playerTracker.UpdateHealth(-diff);
            defendedFor = 0;
            shieldText.text = defendedFor.ToString();
            shieldObject.SetActive(false);
        }
        else
        {
            defendedFor -= amount;
            shieldText.text = defendedFor.ToString();
        }

        healthTxt.text = "HP: " + playerTracker.Health.ToString();

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
