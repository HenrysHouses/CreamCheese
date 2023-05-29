using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FMODUnity;
using EnemyAIEnums;

public class Monster : BoardElement
{

    [SerializeField, HideInInspector]
    public ActionSelection[] EnemyActions;
    public Intent GetIntent() => enemyIntent;
    protected Intent enemyIntent;
    protected int defendFor, queuedDefence;
    private List<ActionCard_Behaviour> targetedByCards;
    public struct TargetedByInfo
    {
        public Monster targetedBy;
        public Color color;
    }
    private List<TargetedByInfo> targetedByEnemies;
    public BoardStateController Board;

    //VFX
    [Header("VFX")]
    public GameObject deathParticleVFX;
    public GameObject slashParticleVFX;
    [SerializeField] Renderer[] MonsterRenderers;
    public float outlineSize = 0.01f;
    private bool outlineShouldTurnOff;
    private float outlineRemainingTime;
    protected int queuedDamage, queuedPoison, queuedPierce;
    protected Dictionary<ActionCard_Behaviour, int> damageSources;

    //UI
    [SerializeField, Header("UI")]
    private Transform effectsPanel;
    [SerializeField]
    private Slider healthBar, poisonBar, barrierBar, afterDamageBar;
    [SerializeField]
    protected Image healthBarFill, barrierBarFill, afterDamageBarFill;
    [SerializeField]
    private TMP_Text healthText, strengthText, defendText, turnsLeftText;
    [SerializeField]
    private Image intentImage;
    [SerializeField]
    private GameObject effectIconPrefab, defendUI, buffImageGameObject;
    private Dictionary<Sprite, GameObject> debuffDisplays;
    protected Color healthBarColor, barrierBarColor;
    [SerializeField]
    private Color damageIndicatorColor;
    private bool flashHealthBar = false, flashBarrierBar = false;

    //Health
    [SerializeField, Header("Health")]
    private int maxHealth;
    private int currentHealth, barrier;
    public int GetMaxHealth() { return maxHealth; }
    public int GetHealth() { return currentHealth; }

    //Effects
    [HideInInspector]
    public bool HealingDisabled, Leached, Weakened, Defending;
    [HideInInspector]
    public int BuffStrength;

    //SFX
    [SerializeField, Header("Sounds")]
    private EventReference death_SFX;
    [SerializeField]
    private EventReference block_SFX;

    // Animation
    [Header("Animation")]
    public Animator animator;
#if UNITY_EDITOR
    public bool KillEnemy;
#endif

    private void Awake()
    {

        maxHealth += Mathf.RoundToInt((float)maxHealth / 10f) * (GameManager.timesDefeatedBoss * 2);

        currentHealth = maxHealth;

        healthBar.maxValue = maxHealth;
        barrierBar.maxValue = maxHealth;
        afterDamageBar.maxValue = maxHealth;

        UpdateHealthUI();

        damageSources = new Dictionary<ActionCard_Behaviour, int>();
        targetedByCards = new List<ActionCard_Behaviour>();
        targetedByEnemies = new List<TargetedByInfo>();
        debuffDisplays = new Dictionary<Sprite, GameObject>();

    }

    protected virtual void Start()
    {

        enemyIntent = new MinionIntent(ref EnemyActions, this);
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

            barrierBar.value = 0;
            afterDamageBarFill.color = healthBarColor;
            healthBarFill.color = Color.Lerp(healthBarColor, damageIndicatorColor, Mathf.PingPong(Time.time, 1));

        }
        else
            healthBarFill.color = healthBarColor;

#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.L))
            TakeDamage(1, true);
        if(KillEnemy)
            StartCoroutine(nameof(Die));
#endif

    }

    public void UpdateQueuedDamage(ActionCard_Behaviour _source, int _amount, bool _buffUpdate, bool _ignoreShield = false)
    {

        if(damageSources.TryGetValue(_source, out int _damage))
        {

            if(_buffUpdate)
            {
                if(_ignoreShield)
                    queuedPierce -= _damage;
                else
                    queuedDamage -= _damage;
                
                damageSources[_source] = 0;
            }

            damageSources[_source] += _amount;
            if(_ignoreShield)
                queuedPierce += _amount;
            else
                queuedDamage += _amount;

        }
        else
        {

            damageSources.Add(_source, _amount);
            targetedByCards.Add(_source);

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

        if(_amount >= 1000)
            StartCoroutine(nameof(Die));

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
            
            animator.SetTrigger("TakingDMGDefended");
            SoundPlayer.PlaySound(block_SFX, gameObject);
            defendFor -= _amount;
            return 0;

        }

        int _leachFor = 0;

        if(barrier > 0) 
        {

            if(barrier - _damageTaken <= 0)
            {

                int _barrierTemp = barrier;
                barrier = 0;
                _damageTaken -= _barrierTemp;

                if(Leached)
                    _leachFor += _barrierTemp;

            }
            else
            {

                if(Leached)
                    _leachFor += _damageTaken;

                barrier -= _damageTaken;
                _damageTaken = 0;

            }

        }

        currentHealth -= _damageTaken;
        setOutline(outlineSize, Color.red, 0.25f);

        if(Leached)
            Board.Player.Heal(currentHealth < 0 ? _damageTaken + currentHealth + _leachFor : _damageTaken + _leachFor);

        if (currentHealth <= 0)
            StartCoroutine(nameof(Die));
        else if(_damageTaken > 0)
            animator.SetTrigger("TakingDMG");

        UpdateHealthUI();
        UpdateDefenceUI();

        return _damageTaken;

    }

    private IEnumerator Die()
    {

        ZeroBars();
        currentHealth = 0;

        for(int i = 0; i < targetedByCards.Count; i++)
            targetedByCards[i].EnemyDied(this);

        for(int i = 0; i < targetedByEnemies.Count; i++)
            targetedByEnemies[i].targetedBy.ReSelectTargets(Board);
        
        animator.SetInteger("RandomDeath", Random.Range(0,3));
        animator.SetTrigger("Dying");
        animator.Update(Time.deltaTime);

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        SoundPlayer.PlaySound(death_SFX,gameObject);
        if (deathParticleVFX != null)
        {
            GameObject spawn = Instantiate(deathParticleVFX, transform.position, Quaternion.identity);
            DestroyOrder test = spawn.GetComponent<DestroyOrder>();
            if (test != null)
            {
                spawn.GetComponent<DestroyOrder>().destroyVFX();
            }
        }

        Destroy(gameObject);

    }

    public void ApplyBarrier(int _amount)
    {

        barrier = _amount;

        UpdateHealthDamageUI();
        UpdateHealthUI();
        setOutline(outlineSize, Color.yellow, 0.25f);
        
    }

    public void ReceiveHealth(int _amount)
    {

        if(HealingDisabled)
            return;

        currentHealth = Mathf.Clamp(currentHealth += _amount, 0, maxHealth);

        UpdateHealthUI();
        setOutline(outlineSize, Color.green, 0.25f);

    }

    public void Weaken(int _amount = 0, bool _onlyOnAttack = false)
    {

        if(_onlyOnAttack && enemyIntent.ActionSelected.ActionIntentType == IntentType.Attack )
            enemyIntent.SetCurrStrengh((int)Mathf.Clamp(enemyIntent.GetCurrStrengh() - _amount, 0, Mathf.Infinity));
        else if(!_onlyOnAttack)
            enemyIntent.SetCurrStrengh((int)Mathf.Clamp(enemyIntent.GetCurrStrengh() / 2, 0, Mathf.Infinity));

        if(enemyIntent.GetID() == EnemyIntent.CleanseEnemy)
            ReSelectTargets(Board);

        UpdateIntentUI();

    }

    public void Buff(int _amount)
    {

        BuffStrength += _amount;

    }

    public void RemoveDebuffs()
    {

        DebuffBase[] _debuffs = GetComponents<DebuffBase>();

        for(int i = 0; i < _debuffs.Length; i++)
        {

            _debuffs[i].RemoveDebuff();

        }

    }

    public DebuffBase TryGetDebuff(System.Type _debuffType)
    {

        DebuffBase[] _debuffs = GetComponents<DebuffBase>();

        for(int i = 0; i < _debuffs.Length; i++)
        {

            if(_debuffs[i].GetType() == _debuffType)
                return _debuffs[i];

        }

        return null;

    }
    
    public bool HasDebuff()
    {

        DebuffBase _debuffCheck = GetComponent<DebuffBase>();

        if(_debuffCheck != null)
            return true;

        return false;

    }

    public bool HasDebuffNextRound()
    {

        DebuffBase[] _debuffs = GetComponents<DebuffBase>();

        for(int i = 0; i < _debuffs.Length; i++)
        {

            if(_debuffs[i].Stacks > 1)
                return true;

        }

        return false;

    }

    protected void ZeroBars()
    {

        healthText.text = (0).ToString();
        healthBar.value = 0;
        barrierBar.value = 0;
        afterDamageBar.value = 0;

    }

    protected void UpdateHealthUI()
    {

        healthText.text = (currentHealth + barrier).ToString();
        healthBar.value = currentHealth;
        barrierBar.value = barrier;
            
    }

    protected void UpdateDefenceUI()
    {

        if(defendFor < 1)
        {

            defendUI.SetActive(false);
            return;

        }

        defendUI.SetActive(true);
        defendText.text = defendFor.ToString();

    }

    protected void UpdateHealthDamageUI()
    {

        flashBarrierBar = false;
        flashHealthBar = false;

        float _damage = (Mathf.Clamp(queuedDamage - defendFor, 0, Mathf.Infinity) + queuedPierce + queuedPoison);
        
        if(_damage - barrier <= 0)
        {

            afterDamageBar.value = barrier - _damage;
            flashBarrierBar = true;
            return;

        }
        else
            _damage -= barrier;

        flashHealthBar = true;
        afterDamageBar.value = currentHealth - _damage;
        
    }

    protected void UpdateIntentUI()
    {

        if(currentHealth <= 0)
            return;

        if(enemyIntent.GetCurrentIcon() == null)
        {

            intentImage.gameObject.SetActive(false);
            strengthText.text = "";
            return;

        }

        strengthText.text = enemyIntent.GetCurrStrengh().ToString();
        intentImage.gameObject.SetActive(true);
        intentImage.sprite = enemyIntent.GetCurrentIcon();
        intentImage.GetComponent<UIPopup>().PopupInfo.Info = enemyIntent.GetCurrentDescription();

        if(enemyIntent.ActionSelected.TurnsLeft > 0)
            turnsLeftText.text = enemyIntent.ActionSelected.TurnsLeft.ToString();
        else
            turnsLeftText.text = "-";

        if(BuffStrength <= 0)
            buffImageGameObject.SetActive(false);
        else
            buffImageGameObject.SetActive(true);

    }

    public void CancelIntent()
    {

        enemyIntent.CancelIntent();
        UpdateIntentUI();

    }

    internal virtual void DecideIntent(BoardStateController board)
    {

        if(Defending)
        {

            defendFor = queuedDefence;
            queuedDefence = 0;
            Defending = false;
            animator.SetBool("isBlocking", true);
            animator.SetTrigger("Blocking");

        }
        else
        {

            defendFor = 0;
            animator.SetBool("isBlocking", false);

        }

        damageSources.Clear();
        queuedDamage = 0;
        queuedPierce = 0;
        queuedPoison = 0;
        UpdateHealthDamageUI();

        enemyIntent.DecideIntent(board);

        UpdateDefenceUI();
        UpdateIntentUI();

    }

    //Just in case a monster needs to know what other enemies will do to decide for itself
    internal void LateDecideIntent(BoardStateController _board)
    {

        enemyIntent.LateDecideIntent(_board);
        UpdateIntentUI();
        BuffStrength = 0;

    }

    public void ReSelectTargets(BoardStateController _board)
    {

        enemyIntent.ActionSelected.SelectTargets(_board);

    }
    
    public void UpdateEffectDisplay(Sprite _icon, int _stacks, string description)
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
            _iconGO.GetComponent<UIPopup>().PopupInfo.Info = description;
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
        else if(targetedByEnemies.Count > 0)
        {
            setOutline(outlineSize, targetedByEnemies[targetedByEnemies.Count-1].color, 10000);
            UpdateOutline();
            return;
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
        outlineRemainingTime -= Time.deltaTime;
        
        if(outlineShouldTurnOff)
            setOutline(0, Color.white);
        else if (outlineRemainingTime <= 0)
        {
            outlineShouldTurnOff = true;
        }
    }

    public void Act(BoardStateController board)
    {

        targetedByEnemies.Clear();
        setOutline(0, Color.white);

        if(enemyIntent != null)
        {
            enemyIntent.Act(board, this);
            TurnController.shouldWaitForAnims = true;
            StartCoroutine(WaitForAnims());
        }

    }

    public void TargetedByEnemy(Monster _enemy, Color _color)
    {

        TargetedByInfo _info = new TargetedByInfo();
        _info.color = _color;
        _info.targetedBy = _enemy;

        targetedByEnemies.Add(_info);

        setOutline(outlineSize, _color, 10000);

    }

    public void PlaySound(EventReference _sfx)
    {

        if(!_sfx.IsNull)
            SoundPlayer.PlaySound(_sfx, gameObject);

    }

    IEnumerator WaitForAnims()
    {
        yield return new WaitForSeconds(1.2f);

        TurnController.shouldWaitForAnims = false;
    }

}