using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{
    public GameObject[] lava;
    public GameObject lavaplain;
    public Transform lavaplainpos1, lavaplainpos2;
    public float lavaplainSpeed;
    public bool turnOnLava = false, testing;
    public HealthCounterController playerHealth;
    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (!testing)
        {

            if (playerHealth.currHealth < 10)
                turnOnLava = true;

            if (playerHealth.currHealth > 10)
                turnOnLava = false;

        }


        if (turnOnLava)
        {
            LavaOn();
            lavaplain.SetActive(true);
            float step = lavaplainSpeed * Time.deltaTime;
            lavaplain.transform.position = Vector3.MoveTowards(lavaplain.transform.position, lavaplainpos2.position, step);

        }
        else
        {
            foreach (GameObject lavas in lava)
            {
                lavas.SetActive(false);
            }
            float step = lavaplainSpeed * Time.deltaTime;
            lavaplain.transform.position = Vector3.MoveTowards(lavaplain.transform.position, lavaplainpos1.position, step);

        }
    }

    public void LavaOn()
    {

        foreach (GameObject lavas in lava)
        {
            lavas.SetActive(true);
        }
    }
}
