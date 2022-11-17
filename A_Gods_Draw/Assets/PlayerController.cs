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

    [SerializeField]
    Image arrowImage;

    // Start is called before the first frame update
    void Start()
    {
        healthTxt.text = "HP: " + playerTracker.Health.ToString();
    }

    // public int GetHealth() { return health; }

    public void DealDamage(int amount)
    {
        Debug.Log("Damage taken:" + -amount);
        playerTracker.UpdateHealth(-amount);

        if (playerTracker.Health < 0)
            playerTracker.Health = 0;

        healthTxt.text = "HP: " + playerTracker.Health.ToString();

    }

    public void Heal(int amount)
    {
        playerTracker.UpdateHealth(amount);

        if (playerTracker.Health > playerTracker.MaxHealth)
            playerTracker.Health = playerTracker.MaxHealth;

        healthTxt.text = "HP: " + playerTracker.Health.ToString();

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
