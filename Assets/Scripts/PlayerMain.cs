using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System.Runtime.InteropServices;

public class PlayerMain : MonoBehaviour
{
    public GameObject CardPrefab;
    // Live player values
    public int health = 30, maxHP = 50;
    public float HPRegenPerSecond = 1;
    public int mana = 30, maxMana = 50;
    public float ManaRegenPerSecond = 1;

    public int chipCount = 0;

    // Temp values, should not be touched
    private float tempHP = 0;
    private float tempMana = 0;
    private float cooldown = 0;

    // Default values below should never be changed unless the player is 'leveling up' or similar
    private int defaultHealth = 30, defaultMaxHP = 50;
    private int defaultMana = 30, defaultMaxMana = 50;
    private float defaultHPRegen = 1, defaultManaRegen = 1;
    
    // --------------

    // Player Status UI
    private Slider HPBar, ManaBar;
    private TextMeshProUGUI HPCounter, ManaCounter;

    // Inventory
    public List<CardData> inventory = new List<CardData>(3);

    // Poker Chip Counter UI
    private TextMeshProUGUI chipCounter;

    // popup
    public GameObject DamagePopUp;

    // Player Controller Script

    private Animator playerAnimator;
    private PlayerController playerControllerScript;
    private BoxCollider2D playerCollider;
    private Rigidbody2D playerRigidBody;
    public GameObject playerWeapon;

    // Game over UI
    public GameOverMenuScript GameOverMenuScript;

    // 
    public AudioManager audioManager;

    List<CardData> possibleCards = new List<CardData> {
    new CardData("Small heal","Heals you a small amount.",12,0),
    new CardData("Large heal","Heals you a significant amount.",35,0),
    new CardData("Small mana","Gives you a small Mana boost.",0,10),
    new CardData("Large mana","Gives you a decent amount of Mana.",0,30),
    new CardData("Vampire","Converts a small amount of Mana into HP",10,-10),
    new CardData("Glass cannon","Turns 20 health into 30 Mana",-20,30),
    new CardData("Boost","Gives a small amount of Mana and HP",12,12),
    // end consumables
    new CardData("Blessing", "You have been blessed by the RNG gods, and all stats have been increased.", 2, 2, 10, 10),
    new CardData("Curse", "You have been cursed! Stats are decreased.", -0.5f , -0.5f , -5, -5),
    new CardData("Eat your veggies", "You have a good diet consisting of vegetables, you regain HP faster.", 3, 0, 0, 0)
    };

    [SerializeField]
    List<Sprite> possibleCardSprites;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPBar = GameObject.Find("HPBar").GetComponent<Slider>();
        ManaBar = GameObject.Find("ManaBar").GetComponent<Slider>();
        HPCounter = GameObject.Find("HPCounter").GetComponent<TextMeshProUGUI>();
        ManaCounter = GameObject.Find("ManaCounter").GetComponent<TextMeshProUGUI>();
        chipCounter = GameObject.Find("ChipCounterText").GetComponent<TextMeshProUGUI>();

        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();
        playerRigidBody = GameObject.Find("Player").GetComponent<Rigidbody2D>();

        GameObject Slot1 = GameObject.Find("Card1Pos");
        GameObject Slot2 = GameObject.Find("Card2Pos");
        GameObject Slot3 = GameObject.Find("Card3Pos");

        foreach (var card in possibleCards)
        {
            card.cardSprite = possibleCardSprites[Random.Range(0, possibleCardSprites.Count)];
        }

        inventory.Add(possibleCards[Random.Range(0, possibleCards.Count)]);
        inventory.Add(possibleCards[Random.Range(0, possibleCards.Count)]);
        inventory.Add(possibleCards[Random.Range(0, possibleCards.Count)]);
        //inventory.Add(new CardData("Example Passive", "+1 HP per second", 1, 0, 0, 0));


        //Takes cards from inventory array and displays them on screen
        foreach (CardData card in inventory)
        {
            GameObject CreatedCard = null;
            if (Slot1.transform.childCount == 0){
                CreatedCard = Instantiate(CardPrefab, Slot1.transform);
            }
            else if (Slot2.transform.childCount == 0){
                CreatedCard = Instantiate(CardPrefab, Slot2.transform);
            }
            else if (Slot3.transform.childCount == 0){
                CreatedCard = Instantiate(CardPrefab, Slot3.transform);
            }
            else{
                Debug.Log("BAD THINGS HAPPENED");
            }

            if (card.consumable){
                CreatedCard.GetComponent<CardGUI>().Initialize(card, card.CardName, card.CardDescription, card.HealAmount, card.ManaAmount);
            }
            else{
                CreatedCard.GetComponent<CardGUI>().Initialize(card, card.CardName, card.CardDescription, card.HPRegenChange, card.manaRegenChange, card.maxHPChange, card.maxManaChange);
            }
        }
    } 

    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown += 0.02f;
        if (cooldown >= 1)
        {
            ResetToDefault(false, false); // reset maxHP to 50
            RunEverySecond(); // Apply card effects (increase maxHP)
            cooldown = 0;
        }
        if(health > 0)
        {
            Regenerate();
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        HPBar.maxValue = maxHP;
        HPBar.value = health;
        HPCounter.text = health + " / " + maxHP;
        
        ManaBar.maxValue = maxMana;
        ManaBar.value = mana;
        ManaCounter.text = mana + " / " + maxMana;

        chipCounter.text = chipCount.ToString();

        foreach(CardData c in inventory)
        {
            if(c.consumed)
            {
                Debug.Log(inventory.Count);
                inventory.Remove(c);
                Debug.Log("Card removed from inventory");
                Debug.Log(inventory.Count);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clearInventory">If all items from inventory should be removed, i.e. for clearing inventory after death</param>
    /// <param name="healToDefault">If current health and mana should be set to initial value</param>
    void ResetToDefault(bool clearInventory, bool healToDefault)
    {
        maxHP = defaultMaxHP;
        maxMana = defaultMaxMana;
        HPRegenPerSecond = defaultHPRegen;
        ManaRegenPerSecond = defaultManaRegen;
        if (clearInventory)
        {
            inventory = new List<CardData>();
        }
        if (healToDefault)
        {
            health = defaultHealth;
            mana = defaultMana;
        }
    }

    void RunEverySecond()
    {
        foreach(CardData card in inventory)
        {
            card.ApplyEffects(this);
        }
    }

    public void TakeDamage(int damageAmount)
    {
        var popUp = Instantiate(DamagePopUp, this.transform.position + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
        popUp.GetComponent<TextMeshPro>().text = damageAmount.ToString();
        health -= damageAmount;

        if(health <= 0)
        {
            Death();
        }
        else
        {
            playerAnimator.SetTrigger("TakingDamage");
        }
    }

    void Death()
    {
        playerControllerScript.enabled = false;
        playerRigidBody.linearVelocity = Vector2.zero;
        
        playerCollider.enabled = false;
        playerWeapon.SetActive(false);

        playerAnimator.SetBool("isDead", true);
        health = 0;

        audioManager.StartDeathTheme();

        GameOverMenuScript.Setup();
    }

    void Regenerate()
    {
        if(health < maxHP)
        {
            tempHP += HPRegenPerSecond * 0.02f;
            if(tempHP >= 1){ 
                health += 1;
                tempHP = 0;
            }}
        if(mana < maxMana){
            tempMana += ManaRegenPerSecond * 0.02f;
            if(tempMana >= 1){ 
                mana += 1;
                tempMana = 0;
            }
        }
        if(health > maxHP){
            health = maxHP;
        }
        if(mana > maxMana){ 
            mana = maxMana;
        }
    }
}
