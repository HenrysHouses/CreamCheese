// Written by Javier

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DitzeGames.Effects;
using UnityEngine.SceneManagement;

public class PlayerController : BoardElement
{

    public int Playerhealth {get{return playerTracker.Health;}}
    public int MaxPlayerHealth {get{return playerTracker.MaxHealth;}}
    [SerializeField] 
    PlayerTracker playerTracker;
    [SerializeField]
    TMP_Text healthTxt;

    [SerializeField]
    Image arrowImage;
    [SerializeField]
    GameObject lightPrefabs;

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
        CameraEffects.ShakeOnce(0.2f,5);
        SceneManager.SetActiveScene(gameObject.scene);
        Instantiate(lightPrefabs, new Vector3(0,0,0), Quaternion.identity);
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
