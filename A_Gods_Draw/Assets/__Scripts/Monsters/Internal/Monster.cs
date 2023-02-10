using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class Monster : BoardElement
{

    public Intent GetIntent() => enemyIntent;
    protected Intent enemyIntent;
    private int defendedFor;

    //VFX
    public GameObject deathParticleVFX;
    public GameObject slashParticleVFX;
    [SerializeField] Renderer[] MonsterRenderers;
    public float outlineSize = 0.01f;
    private bool outlineShouldTurnOff;
    private float outlineRemainingTime;

    //SFX
    private EventReference cardSelect_SFX, death_SFX;
    public EventReference HoverOver_SFX;

    //UI
    [SerializeField]
    private Transform effectsPanel;
    [SerializeField]
    private Slider healthBar, poisonBar, barrierBar;
    [SerializeField]
    private TMP_Text healthText, strengthText;
    [SerializeField]
    private Image intentImage;
    [SerializeField]
    private Icons uiIcons;
    [SerializeField]
    private GameObject effectIconPrefab;
    public enum Effects
    {

        Poison,
        Frostbite,
        HealPrevention,
        Chained

    }

    //Health
    [SerializeField]
    private int maxHealth;
    private int currentHealth, barrier, totalHealthPool;
    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return currentHealth; }

    //Effects
    public bool HealingDisabled, Defending;

    private void Awake()
    {

        maxHealth += Mathf.RoundToInt((float)maxHealth / 10f) * (GameManager.timesDefeatedBoss * 2);

        currentHealth = maxHealth;

        BoardElementInfo = "Hello, I am an enemy";

    }

    private void Update()
    {

        UpdateOutline();

    }

    public void Defend(int amount)
    {
        if (gameObject)
        {
            
            defendedFor += amount;
            Defending = true;

        }
    }

    public int TakeDamage(int _amount, bool bypassDefence = false)
    {

        int _damageTaken = 0;

        if (_amount > defendedFor && !bypassDefence)
        {
            _damageTaken = _amount - defendedFor;
            defendedFor = 0;
            
        }
        else if(bypassDefence)
        {
            
            _damageTaken = _amount;

        }
        else
            defendedFor -= _amount;

        if(barrier > 0)
        {

            _damageTaken -= barrier;

            if(_damageTaken > 0)
                barrier = 0;
            else
                barrier -= _damageTaken;

        }

        if(Defending)
        {

            strengthText.text = defendedFor.ToString();

        }

        currentHealth -= _damageTaken;

        if (currentHealth <= 0)
        {

            currentHealth = 0;
            SoundPlayer.PlaySound(death_SFX,gameObject);
            if (deathParticleVFX != null)
                Instantiate(deathParticleVFX, transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);

        }

        UpdateHealthUI();
        setOutline(outlineSize, Color.red, 0.25f);

        return _damageTaken;

    }

    public void ApplyBarrier(int _amount)
    {

        barrier = _amount;

    }

    public void ReceiveHealth(int _amount)
    {

        if(!HealingDisabled)
            return;

        currentHealth += _amount;

        UpdateHealthUI();
        setOutline(outlineSize, Color.green, 0.25f);

    }

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

        UpdateIntentUI();
    }

    public void Buff(int amount)
    {
        if (enemyIntent.GetID() == EnemyIntent.AttackGod || enemyIntent.GetID() == EnemyIntent.AttackPlayer)
        {
            enemyIntent.SetCurrStrengh(enemyIntent.GetCurrStrengh() + amount);
        }

        UpdateIntentUI();
    }

    private void UpdateHealthUI()
    {

        healthText.text = "HP: " + currentHealth;

        healthBar.value = (currentHealth / maxHealth) * healthBar.maxValue;
        barrierBar.value = (barrier / maxHealth) * barrierBar.maxValue;
            
    }

    private void UpdateIntentUI()
    {

        strengthText.text = enemyIntent.GetCurrStrengh().ToString();
        intentImage.sprite = enemyIntent.GetCurrentIcon();

    }

    internal void DecideIntent(BoardStateController board)
    {
        
        Defending = false;

        enemyIntent.CancelIntent();
        enemyIntent.DecideIntent(board);

        UpdateIntentUI();

    }

    //Just in case a monster needs to know what other enemies will do to decide for itself
    internal void LateDecideIntent(BoardStateController board)
    {

        enemyIntent.LateDecideIntent(board);

        UpdateIntentUI();

    }

    public void ShowEffect(Effects _effect)
    {

        switch(_effect)
        {
            
            case Effects.Poison:
            break;
            
        }

    }

    public void ShowEffect(Sprite _icon)
    {

        GameObject _iconGO = Instantiate(effectIconPrefab, effectsPanel);
        _iconGO.GetComponent<Image>().sprite = _icon;

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

}

struct Icons
{

    //Effect icons
    public Sprite PoisonIcon, FrostbiteIcon, HealPreventionIcon, ChainedIcon;

}