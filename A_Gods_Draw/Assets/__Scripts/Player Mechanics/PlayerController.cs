// Written by Javier

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DitzeGames.Effects;
using UnityEngine.SceneManagement;

public class PlayerController : BoardElement , IMonsterTarget
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
    GameObject[] lights;

    void Start()
    {
        healthTxt.text = "HP: " + playerTracker.Health.ToString();
        UpdateLights();
    }

    public void DealDamage(int amount, UnityEngine.Object _source = null)
    {
        // Debug.Log("Damage taken:" + -amount);
        playerTracker.UpdateHealth(-amount);

        if (playerTracker.Health < 0)
            playerTracker.Health = 0;

        healthTxt.text = "HP: " + playerTracker.Health.ToString();
        CameraEffects.ShakeOnce(0.2f,5);
        SceneManager.SetActiveScene(gameObject.scene);
        UpdateLights();
    }

    public void Heal(int amount)
    {
        playerTracker.UpdateHealth(amount);

        if (playerTracker.Health > playerTracker.MaxHealth)
            playerTracker.Health = playerTracker.MaxHealth;

        healthTxt.text = "HP: " + playerTracker.Health.ToString();

    }

    private void UpdateLights()
    {

        int _count = Mathf.RoundToInt(((float)playerTracker.Health / (float)playerTracker.MaxHealth) * ((float)lights.Length + 1f));
        for(int i = 0; i < lights.Length; i++)
        {

            lights[i].SetActive(_count < i + 1 ? true : false);
            //torches[i].SetActive(_count < i + 1 ? false : true);

        }

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

    public void Targeted(GameObject _sourceGO = null)
    {
        
    }

    // public override string setClassName()
    // {
    //     return GetType().Name;
    // }
}
