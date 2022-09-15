using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    int maxHealth = 10;
    int health;

    int defendedFor = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    void Defend(int value)
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
}
