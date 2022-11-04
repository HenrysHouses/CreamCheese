using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GodPlacement : MonoBehaviour
{
    God_Behaviour god;
    public Image godArrow;
    [SerializeField]
    TMP_Text health;

    public void SetGod(God_Behaviour god)
    {
        //CameraShake.Shake(0.25f, 2f);   // I tried.. 
        this.god = god;
        god.SetPlace(this);
        health.enabled = true;
        health.text = god.CardSO.health.ToString();
    }

    public void RemoveGod()
    {
        god.SetPlace(null);
        god = null;
        health.enabled = false;
    }

    public void UpdateUI()
    {
        health.text = god.Health.ToString();
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
