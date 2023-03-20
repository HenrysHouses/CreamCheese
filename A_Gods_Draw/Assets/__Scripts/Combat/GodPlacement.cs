// Written by Javier Villegas
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Basically just controls the UI showing god health
public class GodPlacement : MonoBehaviour
{
    GodCard_Behaviour god;
    public Image godArrow;
    [SerializeField]
    TMP_Text health;

    public bool GodIsEquals(GodCard_Behaviour target)
    {
        return god.Equals(target);
    }

    public void SetGod(GodCard_Behaviour god)
    {
        //CameraShake.Shake(0.25f, 2f);   // I tried.. 
        this.god = god;
        god.SetPlace(this);
        health.enabled = true;
        health.text = "HP: " + god.CardSO.health.ToString();
    }

    public void RemoveGod()
    {
        god.SetPlace(null);
        god = null;
        health.enabled = false;
    }

    public void UpdateUI()
    {
        health.text = "HP " + god.Health.ToString();
    }

    public void GodShowArrow()
    {
        godArrow.enabled = true;
    }

    public void GodHideArrow()
    {
        godArrow.enabled = false;
    }
}
