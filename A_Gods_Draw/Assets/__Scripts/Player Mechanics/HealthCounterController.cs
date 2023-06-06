/*
 * Written by:
 * Henrik
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

/// <summary>Updates the player's health dial when a change is detected</summary>
public class HealthCounterController : MonoBehaviour
{
    [SerializeField] PlayerTracker player;
    [SerializeField] EventReference HealthTick_SFX;
    [SerializeField] ParamRef HealthTick_Parameter;
    public int currHealth => TotalHealthGained-TotalDamageTaken; 

    int MaxDamageDifference = 100;
    [SerializeField] int TotalHealthGained = 200;
    [SerializeField] int TotalDamageTaken = 100;
    // Health Visualization
    [SerializeField] PathController pathController;
    [SerializeField] Transform HealthObj1, HealthObj2;
    [SerializeField] Transform DamageObj1, DamageObj2;
    
    [SerializeField] float HealthT = 0;
    [SerializeField] float DamageT = 0;
    [SerializeField] float offset;
    [SerializeField] float HealthAnimSpeed = 1;
    [SerializeField] float healingDelay = 0.5f;
    [SerializeField] float GearAnimSpeed = 1;
    // Health Anim
    bool healthIsAnimating = false;


    // Gear Anim
    bool gearIsAnimating = false;
    List<float> shouldTriggerGears = new List<float>();
    public float[] gearSpeed;
    public float[] gearRotateAmount;
    [SerializeField] Transform[] gear;
    [SerializeField] private ParticleSystem particles;

    void Start()
    {
        float DamageAlreadyTaken = (float)(player.MaxHealth - player.Health)/(float)player.MaxHealth;
        TotalDamageTaken += (int)(DamageAlreadyTaken * MaxDamageDifference);
    }

    // Update is called once per frame
    void Update()
    {
        if(HealthObj1 is null || DamageObj1 is null || DamageObj2 is null || HealthObj2 is null || player is null)
            return;

        checkPlayerStatus();

        DoGearAnim();
        DoHealthAnim();
    }

    public void checkPlayerStatus()
    {
        if(!healthIsAnimating && !gearIsAnimating)
        {
            if(player.HealthChanges.Count <= 0)
                return;

            int moveAmount = player.HealthChanges[0];

            // if(moveAmount < 0)
            //     moveAmount *= -1;

            float percent = (float)moveAmount/(float)player.MaxHealth;

            Debug.Log(moveAmount + " / " + player.MaxHealth + " = " + percent);

            Debug.Log((int)(percent * MaxDamageDifference));

            updateHealth((int)(percent * MaxDamageDifference));
            player.HealthChanges.RemoveAt(0);
        }
    }

    bool updateHealth(int healthDifference)
    {
        if(healthDifference < 0 && currHealth != 0)
        {
            TotalDamageTaken = Mathf.Min(TotalDamageTaken + healthDifference * -1, TotalHealthGained);
            TriggerGearAnim(GearAnimSpeed * -1);
            particles.Play();
            return true;
        }

        if(currHealth == player.MaxHealth)
            return false;

        if(healthDifference > 0 && currHealth < player.MaxHealth)
        {
            TotalHealthGained += Mathf.Min(healthDifference, MaxDamageDifference - currHealth);    
            TriggerGearAnim(GearAnimSpeed);
            return true;
        }

        return false;
    }

    void DoHealthAnim()
    {
        if(!healthIsAnimating && (TotalHealthGained != HealthT || TotalDamageTaken != DamageT))
        {
            StartCoroutine(AnimateHealth());
        }
    }

    // dead wolf pos: z = 0.0518
    // healthy wolf pos: z = 0.0176

    IEnumerator AnimateHealth()
    {
        healthIsAnimating = true;

        while(HealthT < TotalHealthGained || DamageT < TotalDamageTaken)
        {   
            // new position
            if(HealthT < TotalHealthGained)
                HealthT += 1;
            if(DamageT < TotalDamageTaken)
                DamageT += 1;

            // Health Positions // ! this code could be shorter


            
            float t1 = HealthT/MaxDamageDifference*2;
            float _Health_1_T = ((t1 * HealthAnimSpeed) + offset)%1; 
            OrientedPoint op = pathController.GetEvenPathOP(_Health_1_T);
            HealthObj1.position = op.pos;
            float _Health_2_T = (t1 * HealthAnimSpeed)%1;
            op = pathController.GetEvenPathOP(_Health_2_T);
            HealthObj2.position = op.pos;

            // Damage Positions
            float t2 = DamageT/MaxDamageDifference*2;
            float _Damage_1_T = ((t2 * HealthAnimSpeed) + offset)%1;
            op = pathController.GetEvenPathOP(_Damage_1_T);
            DamageObj1.position = op.pos;
            
            Vector3 dir = op.rot * DamageObj1.forward;

            float angle = Vector3.SignedAngle(-dir, DamageObj1.right, DamageObj1.forward);  

            while(angle < -10) 
            {
                angle = Vector3.SignedAngle(-dir, DamageObj1.right, DamageObj1.forward);  
                DamageObj1.Rotate(new Vector3(0,0,1), Space.Self);
            }

            float _Damage_2_T = (t2 * HealthAnimSpeed)%1;
            op = pathController.GetEvenPathOP(_Damage_2_T);
            DamageObj2.position = op.pos;

            dir = op.rot * DamageObj2.forward;
            angle = Vector3.SignedAngle(-dir, DamageObj2.right, DamageObj2.forward);  

            while(angle < -10)
            {
                angle = Vector3.SignedAngle(-dir, DamageObj2.right, DamageObj2.forward);  
                DamageObj2.Rotate(new Vector3(0,0,1), Space.Self);
            }

            Vector3 pos1 = DamageObj1.transform.localPosition;
            Vector3 pos2 = DamageObj2.transform.localPosition;
            float zOffset = Mathf.Lerp(0.0518f, 0.0176f, (float)currHealth/(float)MaxDamageDifference);
            pos1.z = zOffset;
            pos2.z = zOffset;
            DamageObj1.transform.localPosition = pos1;
            DamageObj2.transform.localPosition = pos2;

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(healingDelay);
        healthIsAnimating = false;
    }
    
    // Gear Animation
    void TriggerGearAnim(float speed) => shouldTriggerGears.Add(speed);
    void DoGearAnim()
    {
        if(!gearIsAnimating && shouldTriggerGears.Count > 0)
        {
            StartCoroutine(AnimateGears(shouldTriggerGears[0]));
        }
    }
    IEnumerator AnimateGears(float speed)
    {
        gearIsAnimating = true;
        bool animIsDone = false;

        shouldTriggerGears.RemoveAt(0);

        HealthTick_Parameter.Value = player.Health;
        SoundPlayer.PlaySound(HealthTick_SFX, gameObject, HealthTick_Parameter);
        
        float[] currRotationAmount = new float[gear.Length];
        bool[] gearState = new bool[gear.Length];

        while(!animIsDone)
        {
            animIsDone = true;
            for (int i = 0; i < gear.Length; i++)
            {
                if(!gearState[i])
                    gearState[i] = rotateGear(gear[i], gearRotateAmount[i], gearSpeed[i] * speed, ref currRotationAmount[i]);
                if(gearState[i] == false)
                    animIsDone = false;
            }
            yield return new WaitForEndOfFrame();
        }
        gearIsAnimating = false;
    }

    bool rotateGear(Transform transform, float byAngles, float speed, ref float curAnglesRotated)
    {
        float rotation = byAngles * speed * Time.deltaTime;
        curAnglesRotated += rotation;
        transform.Rotate(Vector3.forward * rotation, Space.Self);
        if(curAnglesRotated >= byAngles || curAnglesRotated <= -byAngles)
            return true;
        return false;
    }
}
