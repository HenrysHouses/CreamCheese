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
    private int queuedDamage, queuedPoison, queuedPierce;
    private Dictionary<ActionCard_Behaviour, int> damageSources;

    //UI
    [SerializeField]
    private Transform effectsPanel;
    [SerializeField]
    private Slider healthBar, poisonBar, barrierBar, afterDamageBar;
    [SerializeField]
    private Image healthBarFill, barrierBarFill, afterDamageBarFill;
    [SerializeField]
    private TMP_Text healthText, strengthText, defendText;
    [SerializeField]
    private Image intentImage;
    [SerializeField]
    private Icons uiIcons;
    [SerializeField]
    private GameObject effectIconPrefab, defendUI;
    private Dictionary<Sprite, GameObject> debuffDisplays;
    private Color healthBarColor, barrierBarColor;
    [SerializeField]
    private Color damageIndicatorColor;
    private bool flashHealthBar, flashBarrierBar;

    //Health
    [SerializeField]
    private int maxHealth;
    private int currentHealth, barrier;
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
        afterDamageBar.maxValue = maxHealth;

        UpdateHealthUI();

        BoardElementInfo = "Hello, I am an enemy";

        damageSources = new Dictionary<ActionCard_Behaviour, int>();
        debuffDisplays = new Dictionary<Sprite, GameObject>();

    }

    private void Start()
    {

        enemyIntent = new LokiMonster2Intent();
        enemyIntent.Self = this;
        healthBarColor = healthBarFill.color;
        barrierBarColor = barrierBarFill.color;

    }

    private void Update()
    {

        UpdateOutline();

        if(flashBarrierBar)
        {

            afterDamageBarFill.color = barrierBarColor;
            barrierBarFill.color = Color.Lerp(barrierBarColor, damageIndicatorColor, Mathf.PingPong(Time.time, 1));
        
        }
        else
            barrierBarFill.color = barrierBarColor;
        

        if(flashHealthBar)
        {

            afterDamageBarFill.color = healthBarColor;
            healthBarFill.color = Color.Lerp(healthBarColor, damageIndicatorColor, Mathf.PingPong(Time.time, 1));

        }
        else
            healthBarFill.color = healthBarColor;

    }

    public void UpdateQueuedDamage(ActionCard_Behaviour _source, int _amount, bool _ignoreShield = false)
    {

        if(damageSources.TryGetValue(_source, out int _damage))
        {

            if(_ignoreShield)
                queuedPierce -= _damage;
            else
                queuedDamage -= _damage;

            damageSources[_source] = _amount;
            if(_ignoreShield)
                queuedPierce += _amount;
            else
                queuedDamage += _amount;

        }
        else
        {

            damageSources.Add(_source, _amount);

            if(_ignoreShield)
                queuedPierce += _amount;
            else
                queuedDamage += _amount;

        }

        UpdateHealthDamageUI();

    }

    public void UpdateQueuedPoison(int _amount)
    {

        queuedPoison = _amount;

        UpdateHealthDamageUI();

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
            _damageTaken = _amount;
        else
        {

            defendFor -= _amount;
            return 0;

        }

        if(barrier > 0)
        {

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

        UpdateHealthUI();
        setOutline(outlineSize, Color.yellow, 0.25f);

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

        healthText.text = currentHealth + "/" + maxHealth;

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

    private void UpdateHealthDamageUI()
    {

        flashBarrierBar = false;
        flashHealthBar = false;

        float _damage = (Mathf.Clamp(queuedDamage - defendFor, 0, Mathf.Infinity) + queuedPierce + queuedPoison);
        _damage -= barrier;

        if(_damage <= 0)
        {

            afterDamageBar.value = barrier - _damage;
            flashBarrierBar = true;
            return;

        }

        flashHealthBar = true;
        afterDamageBar.value = currentHealth - _damage;
        
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
        queuedPierce = 0;
        queuedPoison = 0;

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

    
    public void UpdateEffect(Sprite _icon, int _stacks)
    {

        GameObject _iconGO;

        if(debuffDisplays.TryGetValue(_icon, out _iconGO))
        {

            if(_stacks <= 0)
            {

                _iconGO.SetActive(false);

            }
            else
            {

                _iconGO.SetActive(true);

            }

            _iconGO.GetComponentInChildren<TMP_Text>().text = _stacks.ToString();

        }
        else
        {
            
            _iconGO = Instantiate(effectIconPrefab, effectsPanel);
            _iconGO.GetComponent<Image>().sprite = _icon;
            _iconGO.GetComponentInChildren<TMP_Text>().text = _stacks.ToString();

            debuffDisplays.Add(_icon, _iconGO);

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

}
[SerializeField]
public struct Icons
{

    //Effect icons
    public Sprite PoisonIcon, FrostbiteIcon, HealPreventionIcon, ChainedIcon;

}