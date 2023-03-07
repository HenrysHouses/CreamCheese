using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DitzeGames.Effects;

public class LokiDeath : MonoBehaviour
{
    private SpriteRenderer lokiSprite;
    public bool killedLoki,done;
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        lokiSprite = GetComponent<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("KilledLoki",false);
    }

    // Update is called once per frame
    void Update()
    {
        if(killedLoki && !done)
        {
            anim.SetBool("KilledLoki",true);
            done = true;
            if(done)
            {
                CameraEffects.ShakeOnce(3,5);
            }
        }
        
    }
}
