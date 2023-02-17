using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;

public class Monster : BoardElement
{

    public Intent GetIntent() => enemyIntent;
    protected Intent enemyIntent;
    private int defendFor, queuedDefence;

    //VFX
    public GameObject deathParticleVFX;
    public GameObject slashParticleVFX;
    [SerializeField] Renderer[] MonsterRenderers;
    public float outlineSize = 0.01f;
    private bool outlineShouldTurnOff;
    private float outlineRemainingTime;
    private int queuedDamage;
    private Dictionary<ActionCard_Behaviour, int> damageSources;

    //UI
    [SerializeField]
    private Transform effectsPanel;
    [SerializeField]
    private Slider healthBar, poisonBar, barrierBar;
    [SerializeField]
    private TMP_Text healthText, strengthText, queuedDamageText, defendText;
    [SerializeField]
    private Image intentImage;
    [SerializeField]
    private Icons uiIcons;
    [SerializeField]
    private GameObject effectIconPrefab, defendUI;
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

    //SFX
    [SerializeField]
    private EventReference death_SFX, block_SFX;
    public EventReference HoverOver_SFX;

    private void Awake()
    {

        maxHealth += Mathf.RoundToInt((float)maxHealth / 10f) * (GameManager.timesDefeatedBoss * 2);

        currentHealth = maxHealth;

        healthBar.maxValue = maxHealth;
        barrierBar.maxValue = maxHealth;

        UpdateHealthUI();

        BoardElementInfo = "Hello, I am an enemy";

        damageSources = new Dictionary<ActionCard_Behaviour, int>();

    }

    private void Start()
    {

        enemyIntent = new LokiMonster2Intent();
        enemyIntent.Self = this;

    }

    private void Update()
    {

        UpdateOutline();
        queuedDamageText.text = "Q: " + Mathf.Clamp(queuedDamage - defendFor, 0, Mathf.Infinity);

    }

    public void UpdateQueuedDamage(ActionCard_Behaviour _source, int _amount)
    {

        if(damageSources.TryGetValue(_source, out int _damage))
        {

            queuedDamage -= _damage;

            damageSources[_source] = _amount;
            queuedDamage += _amount;

        }
        else
        {

            damageSources.Add(_source, _amount);
            queuedDamage += _amount;

        }

    }

    public void Defend(int _amount)
    {

        queuedDefence += _amount;
        Defending = true;
        
    }

    public int TakeDamage(int _amount, bool _bypassDefence = false)
    {

        int _damageTaken = 0;

        if (_amount > defendFor && !_bypassDefence)
        {
            _damageTaken = _amount - defendFor;
            defendFor = 0;
            
        }
        else if(_bypassDefence)
        {
            
            _damageTaken = _amount;

        }
        else
            defendFor -= _amount;

        if(barrier > 0)
        {

            _damageTaken -= barrier;

            if(_damageTaken > 0)
                barrier = 0;
            else
                barrier -= _damageTaken;

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
        UpdateDefenceUI();
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

    public void DeBuff(int _amount)
    {

        enemyIntent.SetCurrStrengh((int)Mathf.Clamp(enemyIntent.GetCurrStrengh() - _amount, 0, Mathf.Infinity));

        UpdateIntentUI();

    }

    public void Buff(int _amount)
    {
        if (enemyIntent.GetID() == EnemyIntent.AttackGod || enemyIntent.GetID() == EnemyIntent.AttackPlayer)
        {
            enemyIntent.SetCurrStrengh(enemyIntent.GetCurrStrengh() + _amount);
        }

        UpdateIntentUI();
    }

    private void UpdateHealthUI()
    {

        healthText.text = "HP: " + currentHealth;

        healthBar.value = currentHealth;
        barrierBar.value = barrier;
            
    }

    private void UpdateDefenceUI()
    {

        if(defendFor < 1)
        {

            defendUI.SetActive(false);
            return;

        }

        defendUI.SetActive(true);
        defendText.text = defendFor.ToString();

    }

    private void UpdateIntentUI()
    {

        strengthText.text = enemyIntent.GetCurrStrengh().ToString();
        intentImage.sprite = enemyIntent.GetCurrentIcon();

    }

    internal void DecideIntent(BoardStateController board)
    {

        if(Defending)
        {

            defendFor = queuedDefence;
            queuedDefence = 0;
            Defending = false;

        }
        else
            defendFor = 0;

        damageSources.Clear();
        queuedDamage = 0;

        enemyIntent.CancelIntent();
        enemyIntent.DecideIntent(board);

        UpdateDefenceUI();
        UpdateIntentUI();

    }

    //Just in case a monster needs to know what other enemies will do to decide for itself
    internal void LateDecideIntent(BoardStateController _board)
    {

        enemyIntent.LateDecideIntent(_board);

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