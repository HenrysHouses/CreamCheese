using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LokiMonster3 : IMonster
{
    private void Start()
    {
        enemyIntent = new LokiMonster3Intent();
    }

}
