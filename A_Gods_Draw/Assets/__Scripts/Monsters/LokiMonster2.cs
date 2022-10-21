
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LokiMonster2 : IMonster
{
    private void Start()
    {
        enemyIntent = new LokiMonster2Intent();
    }
}
