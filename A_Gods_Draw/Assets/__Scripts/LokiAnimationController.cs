//Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LokiAnimationController : MonoBehaviour
{
    private Animator lokiAnimator;

    TurnController controller;
    bool EncounterName;

    bool hasWon;
    bool hasLaughed;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<TurnController>();
        lokiAnimator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.isCombatStarted)
        {
            return;
        }

        EncounterName = controller.GetBoard().isEnemyDefeated;
        if (EncounterName && !hasWon)
        {
            lokiAnimator.SetTrigger("encounterWon");
            hasWon = true;
        }
        else
        {
            hasWon = false;
        }

        LokiLaughing();
    }

    private void LokiLaughing()
    {
        if(!hasLaughed)
        {
            lokiAnimator.SetTrigger("randomLaugh");
            hasLaughed = true;
        }
    }
}
