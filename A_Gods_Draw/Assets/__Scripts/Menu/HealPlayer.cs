/*
 * Written by:
 * Henrik
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    [SerializeField] PlayerTracker player;

    public void healPlayer(int HealAmount)
    {
        if (player.Health < player.MaxHealth)
            player.UpdateHealth(HealAmount);

        if (player.Health >= 30)
        {
            player.Health = 30;
            player.UpdateHealth(30);

        }
    }
}
