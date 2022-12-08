// Written by Javier Villegas
// modified by charlie

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public abstract class IMonster : BoardElement
{
    public Intent GetIntent() => enemyIntent;
    protected Intent enemyIntent;

    public GameObject deathParticleVFX;
    public GameObject slashParticleVFX;
    [SerializeField] Renderer[] MonsterRenderers;
    public float outlineSize = 0.01f;
    bool outlineShouldTurnOff;
    float outlineRemainingTime;

    [SerializeField]
    int maxHealth;
    int health;

    int weakened = 0;

    [SerializeField]
    private EventReference SoundSelectCard,death_SFX;
    public EventReference hoverOver_SFX;

    int defendedFor;

    [SerializeField]
    Image image;
    [SerializeField]
    TMP_Text strengh;
    [SerializeField]
    TMP_Text healthTxt;
    [SerializeField]
    TMP_Text defendTxt;

    [SerializeField]
    Sprite attackIcon;

    Image overlay;

    [SerializeField]
    Sprite abilityIcon;

    [SerializeField]
    Image arrowImage;

    void Awake()
    {
        //Quick formula for scaling difficulty
        maxHealth += Mathf.RoundToInt((float)maxHealth / 10f) * (GameManager.timesDefeatedBoss * 2);

        health = maxHealth;

        weakened = 0;

        healthTxt.text = "HP: " + health.ToString();

        image.enabled = false;
        strengh.enabled = false;

        defendTxt.enabled = false;

        overlay = transform.GetChild(1).GetComponent<Canvas>().gameObject.AddComponent<Image>();
        overlay.enabled = false;
    }

    private void Update() 
    {
        UpdateOutline();
    }


    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return health; }

    public void DealDamage(int amount)
    {
        if (weakened > 0)
            amount++;

        if (amount >= defendedFor)
        {
            health = health - (amount - defendedFor);
            defendedFor = 0;
            image.color = Color.white;
            defendTxt.enabled = false;
        }
        else
        {
            defendedFor -= amount;
        }

        defendTxt.text = defendedFor.ToString();

        if (health <= 0)
        {
            health = 0;
            SoundPlayer.PlaySound(death_SFX,gameObject);
            if (deathParticleVFX != null)
                Instantiate(deathParticleVFX,image.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        healthTxt.text = "HP: " + health.ToString();
        setOutline(outlineSize, Color.red, 0.25f);

    }

    internal void DecideIntent(BoardStateController board)
    {
        enemyIntent.CancelIntent();

        if (weakened > 0)
            weakened--;

        enemyIntent.DecideIntent(board);
    }

    //Just in case a monster needs to know what other enemies will do to decide for itself
    internal void LateDecideIntent(BoardStateController board)
    {
        enemyIntent.LateDecideIntent(board);

        UpdateUI();
        overlay.enabled = false;
    }

    //Some overlay for any action that needs to be visualized (used for chains)
    public void SetOverlay(Sprite sprite)
    {
        overlay.sprite = sprite;
        overlay.enabled = true;
    }

    public void Weaken(int amount)
    {
        weakened += amount;
    }

    public void Defend(int amount)
    {
        if (gameObject)
        {
            defendedFor += amount;
            image.color = Color.cyan;
            defendTxt.text = defendedFor.ToString();
            defendTxt.enabled = true;
        }
    }

    //for defend cards to reduce the damage of attacks
    public void DeBuff(int amount)
    {
        if (enemyIntent.GetID() == EnemyIntent.AttackGod || enemyIntent.GetID() == EnemyIntent.AttackPlayer)
        {
            enemyIntent.SetCurrStrengh(enemyIntent.GetCurrStrengh() - amount);
            if (enemyIntent.GetCurrStrengh() < 0)
            {
                enemyIntent.SetCurrStrengh(0);
            }
        }

        UpdateUI();
    }

    public void Buff(int amount)
    {
        if (enemyIntent.GetID() == EnemyIntent.AttackGod || enemyIntent.GetID() == EnemyIntent.AttackPlayer)
        {
            enemyIntent.SetCurrStrengh(enemyIntent.GetCurrStrengh() + amount);
        }

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (strengh)
        {
            strengh.text = enemyIntent.GetCurrStrengh().ToString();
            strengh.enabled = true;
        }
        if (image)
        {
            image.sprite = enemyIntent.GetCurrentIcon();
            image.GetComponent<ShowDescription>().SetText(enemyIntent.GetCurrentDescription());
            image.enabled = true;
        }
    }

    public void setOutline(float size, Color color, float duration = 0)
    {
        if(size > 0)
        {
            outlineShouldTurnOff = false;
            outlineRemainingTime = duration;
        }

        foreach (var rend in MonsterRenderers)
        {
            if(rend.materials.Length > 1)
            {
                rend.materials[1].SetFloat("_Size", size);            
                rend.materials[1].SetColor("_Color", color);            
            }
        }
    }

    private void UpdateOutline()
    {
        outlineRemainingTime = Mathf.Clamp01(outlineRemainingTime - Time.deltaTime);
        
        if(outlineShouldTurnOff)
            setOutline(0, Color.white);
        else if (outlineRemainingTime == 0)
        {
            outlineShouldTurnOff = true;
        }
    }

    public void Act(BoardStateController board)
    {
        enemyIntent.Act(board, this);
        StartCoroutine(WaitForAnims());
    }

    IEnumerator WaitForAnims()
    {
        yield return new WaitForSeconds(0.5f);

        TurnController.shouldWaitForAnims = false;
    }

    private void OnMouseOver()
    {
        arrowImage.color = Color.red;
    }

    private void OnMouseExit()
    {
        arrowImage.color = Color.white;
    }

    public void EnemyShowArrow()
    {
    }

    public void EnemyHideArrow()
    {
        arrowImage.enabled = false;
    }
}
