using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GodPlacement : MonoBehaviour
{
    [SerializeField]
    TMP_Text healthTxt;

    int defendedFor = 0;

    Defense_Behaviour defender;
    TurnManager manager;

    public Image arrowImage;
    [SerializeField]
    GameObject shieldObject;

    TMP_Text shieldText;

    God_Behaviour god;

    public void SetGod(God_Behaviour god)
    {
        this.god = god;
        god.SetPlace(this);
    }

    public void GodShowArrow()
    {
        arrowImage.enabled = true;
    }

    public void GodHideArrow()
    {
        arrowImage.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        //health = maxHealth;
        //healthTxt.text = "HP: " + health.ToString();
        shieldText = shieldObject.transform.GetChild(0).GetComponentInChildren<TMP_Text>();
    }

    //public int GetHealth() { return }

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

    private void OnMouseDown()
    {
        if (defender)
        {
            shieldObject.SetActive(true); //needs to be able to change the value of how much its shielding for
            defender.ItDefends(); //needs to add the god
            GodHideArrow();
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
}
