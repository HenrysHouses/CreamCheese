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
    private CameraMovement cam;
    public lowhealthflash damageflash;

    void Start()
    {
        healthTxt.text = "HP: " + playerTracker.Health.ToString();
        cam = Camera.main.GetComponent<CameraMovement>();
        damageflash = GameObject.Find("LowHealthFlash").GetComponent<lowhealthflash>();
    }

    public void DealDamage(int amount, UnityEngine.Object _source = null)
    {
        
        // Debug.Log("Damage taken:" + -amount);
        playerTracker.UpdateHealth(-amount);

        if (playerTracker.Health < 0)
            playerTracker.Health = 0;

        Debug.LogError("Camera shakes and take dmg shows even tho ur not taking dmg");
        //TakeDamageCamera(); // Sets the camera to the healhtdial //TODO: setr this somewhere where the player takes dmg
        healthTxt.text = "HP: " + playerTracker.Health.ToString();
        SceneManager.SetActiveScene(gameObject.scene);
        GameManager.instance.EffectIntensity = (float)playerTracker.Health / (float)playerTracker.MaxHealth;

        cam.SetCameraView(CameraView.Reset); // Resets the camera view after taking damage
        if(amount <= 0)
            return;

        CameraEffects.ShakeOnce(0.2f,5);
        damageflash.flashRedWhenHit = true;

    }

    public void Heal(int amount)
    {
        playerTracker.UpdateHealth(amount);

        if (playerTracker.Health > playerTracker.MaxHealth)
            playerTracker.Health = playerTracker.MaxHealth;

        healthTxt.text = "HP: " + playerTracker.Health.ToString();

    }

    private void TakeDamageCamera()
    {

        cam.SetCameraView(CameraView.TakingDamage);
        

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

    public void UnTargeted(GameObject _sourceGO = null)
    {
        
    }

    // public override string setClassName()
    // {
    //     return GetType().Name;
    // }
}
