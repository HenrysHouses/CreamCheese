/*
 * Written by:
 * Henrik
 * 
 */

using UnityEngine;

public class HealPlayer : MonoBehaviour
{
    [SerializeField] PlayerTracker player;

    public void healPlayer(int HealAmount)
    {
        Map.Map_Manager.SavingMap();
        if (player.Health < player.MaxHealth)
            player.UpdateHealth(HealAmount);
        player.SavePlayerData();
    }
}
