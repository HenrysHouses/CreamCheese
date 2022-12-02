using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : IMonster
{
    private void Start()
    {
        enemyIntent = new BossIntent();
    }

}
