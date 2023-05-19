using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerDataContainer
{

    public int TimesDefeatedBoss, PlayerHealth;
    public RuneData[] Runes;

    public PlayerDataContainer(int _timesDefeatedBoss, int _playerHealth, RuneData[] _runes)
    {

        TimesDefeatedBoss = _timesDefeatedBoss;
        PlayerHealth = _playerHealth;
        Runes = _runes;

    }

}