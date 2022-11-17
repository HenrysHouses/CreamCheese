using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HealthCounterController : MonoBehaviour
{
    [SerializeField] PlayerTracker player;
    [SerializeField] EventReference HealthTick_SFX;
    [SerializeField] ParamRef HealthTick_Parameter;
    public int currHealth => Health-Damage; 
    /// <summary>how many health points the player has had</summary> // this is offset by 50
    public int MaxHealth = 100;
    int Health = 200;
    /// <summary>how much total damage the player has taken</summary> // this is offset by 50
    int Damage = 100;
    // Health Visualization
    [SerializeField] PathController pathController;
    [SerializeField] Transform HealthObj1, HealthObj2;
    [SerializeField] Transform DamageObj1, DamageObj2;
    
    float HealthT = 0;
    float DamageT = 0;
    [SerializeField] float offset;
    [SerializeField] float HealthAnimSpeed = 1;
    [SerializeField] float healingDelay = 0.5f;
    [SerializeField] float GearAnimSpeed = 1;
    // Health Anim
    bool healthIsAnimating = false;
    int shouldTriggerHealth = 0;


    // Gear Anim
    bool gearIsAnimating = false;
    List<float> shouldTriggerGears = new List<float>();
    public float[] gearSpeed;
    public float[] gearRotateAmount;
    [SerializeField] Transform[] gear;

    void Start()
    {
        initPlayerHealth();
        MaxHealth = player.MaxHealth;
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

    void initPlayerHealth()
    {
        Health = MaxHealth * 2;
        Damage = Health - player.Health;
    }

    void checkPlayerStatus()
    {
        if(!healthIsAnimating && !gearIsAnimating)
        {
            if(player.HealthChanges.Count <= 0)
                return;

            updateHealth(player.HealthChanges[0]);
            player.HealthChanges.RemoveAt(0);
        }
    }

    bool updateHealth(int healthDifference)
    {
        if(healthDifference < 0 && currHealth != 0)
        {

            Damage = Mathf.Min(Damage + healthDifference * -1, Health);
            TriggerGearAnim(GearAnimSpeed * -1);
            return true;
        }

        if(currHealth < 0 && currHealth >= 100)
            return false;

        if(healthDifference > 0 && currHealth < 100)
        {
            Health += Mathf.Min(healthDifference, MaxHealth - currHealth);    
            TriggerGearAnim(GearAnimSpeed);
            return true;
        }

        return false;
    }

    void DoHealthAnim()
    {
        if(!healthIsAnimating && (Health != HealthT || Damage != DamageT))
        {
            StartCoroutine(AnimateHealth());
        }
    }

    // dead wolf pos: z = 0.0518
    // healthy wolf pos: z = 0.0176

    IEnumerator AnimateHealth()
    {
        healthIsAnimating = true;

        while(HealthT < Health || DamageT < Damage)
        {   
            // new position
            if(HealthT < Health)
                HealthT += 1;
            if(DamageT < Damage)
                DamageT += 1;

            // Health Positions // ! this code could be shorter
            OrientedPoint op = pathController.GetEvenPathOP((((HealthT/(MaxHealth*2)) * HealthAnimSpeed) + offset)%1);
            HealthObj1.position = op.pos;
            op = pathController.GetEvenPathOP(((HealthT/(MaxHealth*2)) * HealthAnimSpeed)%1);
            HealthObj2.position = op.pos;

            // Damage Positions
            op = pathController.GetEvenPathOP(((DamageT/(MaxHealth*2) * HealthAnimSpeed) + offset)%1);
            DamageObj1.position = op.pos;
            
            Vector3 dir = op.rot * DamageObj1.forward;

            float angle = Vector3.SignedAngle(-dir, DamageObj1.right, DamageObj1.forward);  

            while(angle < -10) 
            {
                angle = Vector3.SignedAngle(-dir, DamageObj1.right, DamageObj1.forward);  
                DamageObj1.Rotate(new Vector3(0,0,1), Space.Self);
            }

            op = pathController.GetEvenPathOP(((DamageT/(MaxHealth*2)) * HealthAnimSpeed)%1);
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
            float zOffset = Mathf.Lerp(0.0518f, 0.0176f, (float)currHealth/(float)MaxHealth);
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
