using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    int maxHealth = 10;
    int health;

    int defendedFor = 0;

    Defense_Behaviour defender;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void CanBeDefended(Defense_Behaviour beh)
    {
        defender = beh;
    }

    public void Defend(int value)
    {
        defendedFor += value;
    }

    void DealDamage(int amount)
    {
        if (amount > defendedFor)
            health = health - (amount - defendedFor);

        if (health <= 0)
        {
            Debug.Log("You Died lmao noob");
        }
    }

    private void OnMouseDown()
    {
        if (defender)
        {
            defender.ItDefends(this);
            Debug.Log(defender + " is going to defend this");
        }
        defender = null;
    }
}
