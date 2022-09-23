//charlie

using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class Card_ClickGlowing : MonoBehaviour
{
    public GameObject glowBorder;
    bool canSpawn;

    public void Glowing()
    {
        if(Input.GetMouseButtonDown(0) && !canSpawn)
        {
            glowBorder.SetActive(true);
            canSpawn = true;
        }
        else
        {
            glowBorder.SetActive(false);
        }
    }
}
