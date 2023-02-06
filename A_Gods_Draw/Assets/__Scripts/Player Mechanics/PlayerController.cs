// Written by Javier

using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : BoardElement
{
    [SerializeField] 
    PlayerTracker playerTracker;
    [SerializeField]
    TMP_Text healthTxt;

    [SerializeField]
    Image arrowImage;

    void Start()
    {
        healthTxt.text = "HP: " + playerTracker.Health.ToString();
    }

    public void DealDamage(int amount)
    {
        // Debug.Log("Damage taken:" + -amount);
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

    // public override string setClassName()
    // {
    //     return GetType().Name;
    // }
}
