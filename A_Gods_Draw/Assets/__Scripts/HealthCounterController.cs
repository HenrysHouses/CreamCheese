using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HealthCounterController : MonoBehaviour
{
    [SerializeField] EventReference HealthTick_SFX;
    public int currHealth => Health-Damage; 
    public int Health = 100;
    public int MaxHealth = 100;
    public int Damage = 0;
    public float HealthScale = 30;
    // Health Visualization
    [SerializeField] PathController pathController;
    [SerializeField] Transform HealthObj1, HealthObj2;
    [SerializeField] Transform DamageObj1, DamageObj2;
    
    public float HealthT;
    public float DamageT;
    [SerializeField] float wolfTestOffset;
    [SerializeField] float offset;
    [SerializeField] float speed;
    // Health Anim
    bool healthIsAnimating = false;
    int shouldTriggerHealth = 0;
    [SerializeField] float healthAnimSpeed = 1;


    // Gear Anim
    bool gearIsAnimating = false;
    [SerializeField] int shouldTriggerGears = 0;
    public float[] gearSpeed;
    public float[] gearRotateAmount;
    [SerializeField] Transform[] gear;

    // Start is called before the first frame update
    void Start()
    {
        DamageT = wolfTestOffset;
        TriggerGearAnim();
    }

    // Update is called once per frame
    void Update()
    {
        if(HealthObj1 is null || DamageObj1 is null || DamageObj2 is null || HealthObj2 is null )
            return;

        DoHealthAnim();
        DoGearAnim();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerGearAnim();
            updateHealth(-10);
        }
    }

    void updateHealth(int healthDifference)
    {
        if(currHealth > 0 && currHealth <= 100)
        {
            if(healthDifference > 0 && currHealth < 100)
                Health += Mathf.Clamp(healthDifference, 0, MaxHealth - currHealth);
            else
                Damage += healthDifference * -1;
        }
        else
            Debug.LogWarning("player has max health");
    }

    void DoHealthAnim()
    {
        if(!healthIsAnimating && Health != HealthT)
        {
            StartCoroutine(AnimateHealth());
        }
    }

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
            // wolf positioning
            // float damagePercent = (float)((float)Health - (float)Damage)/(float)Health;
            // float damageTotal = ExtensionMethods.Remap(damagePercent, 0, 1, 0, ((MaxHealth-HealthScale)/MaxHealth));
            // DamageT =  Health - (MaxHealth/2 * damageTotal);

            // Health Positions
            OrientedPoint op = pathController.GetEvenPathOP((((HealthT/MaxHealth) * speed) + offset)%1);
            HealthObj1.position = op.pos;
            Debug.Log(HealthT);
            op = pathController.GetEvenPathOP(((HealthT/MaxHealth) * speed)%1);
            HealthObj2.position = op.pos;

            // Damage Positions
            op = pathController.GetEvenPathOP(((DamageT/MaxHealth * speed) + offset)%1);
            DamageObj1.position = op.pos;
            
            Vector3 dir = op.rot * DamageObj1.forward;

            float angle = Vector3.SignedAngle(-dir, DamageObj1.right, DamageObj1.forward);  

            // Debug.DrawLine(DamageObj1.position, DamageObj1.right + DamageObj1.position, Color.blue, 0.1f);
            // Debug.DrawLine(DamageObj1.position, DamageObj1.position - dir, Color.red, 0.1f);

            if(angle < -10) // ! make work in both directions
            {
                // float turnDir = angle/angle;
                angle = Vector3.SignedAngle(-dir, DamageObj1.right, DamageObj1.forward);  
                DamageObj1.Rotate(new Vector3(0,0,1), Space.Self);
            }

            
            op = pathController.GetEvenPathOP(((DamageT/MaxHealth) * speed)%1);
            DamageObj2.position = op.pos;

            dir = op.rot * DamageObj2.forward;
            angle = Vector3.SignedAngle(-dir, DamageObj2.right, DamageObj2.forward);  
            // Debug.Log(angle);      

            if(angle < -10)
            {
                // float turnDir = angle/angle;
                angle = Vector3.SignedAngle(-dir, DamageObj2.right, DamageObj2.forward);  
                DamageObj2.Rotate(new Vector3(0,0,1), Space.Self);
            }

            // Debug.DrawLine(DamageObj2.position, DamageObj2.right + DamageObj2.position, Color.blue, 0.1f);
            // Debug.DrawLine(DamageObj2.position, DamageObj2.position - dir, Color.red, 0.1f);
            
            yield return new WaitForEndOfFrame();
        }

        healthIsAnimating = false;
        Debug.Log("healed");
    }
    
    // Gear Animation
    void TriggerGearAnim() => shouldTriggerGears+=1;
    void DoGearAnim()
    {
        if(!gearIsAnimating && shouldTriggerGears > 0)
        {
            StartCoroutine(AnimateGears());
            shouldTriggerGears--;
        }
    }
    IEnumerator AnimateGears()
    {
        Debug.Log("something starts");
        gearIsAnimating = true;
        bool animIsDone = false;
        SoundPlayer.Playsound(HealthTick_SFX, gameObject);
        float[] currRotationAmount = new float[gear.Length];
        bool[] gearState = new bool[gear.Length];

        while(!animIsDone)
        {
            animIsDone = true;
            for (int i = 0; i < gear.Length; i++)
            {
                if(!gearState[i])
                    gearState[i] = rotateGear(gear[i], gearRotateAmount[i], gearSpeed[i], ref currRotationAmount[i]);
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
