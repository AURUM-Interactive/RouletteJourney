using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMain : MonoBehaviour
{
    // Live player values
    public int health = 30, maxHP = 50;
    public float HPRegenPerSecond = 1;
    public int mana = 30, maxMana = 50;
    public float ManaRegenPerSecond = 1;

    // Temp values, should not be touched
    private float tempHP = 0;
    private float tempMana = 0;
    private float cooldown = 0;

    // Default values below should never be changed unless the player is 'leveling up' or similar
    private int defaultHealth = 30, defaultMaxHP = 50;
    private int defaultMana = 30, defaultMaxMana = 50;
    private float defaultHPRegen = 1, defaultManaRegen = 1;
    
    // --------------

    private Slider HPBar, ManaBar;
    private TextMeshProUGUI HPCounter, ManaCounter;

    public List<Card> inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPBar = GameObject.Find("HPBar").GetComponent<Slider>();
        ManaBar = GameObject.Find("ManaBar").GetComponent<Slider>();
        HPCounter = GameObject.Find("HPCounter").GetComponent<TextMeshProUGUI>();
        ManaCounter = GameObject.Find("ManaCounter").GetComponent<TextMeshProUGUI>();
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
        Regenerate();
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
            inventory = new List<Card>();
        }
        if (healToDefault)
        {
            health = defaultHealth;
            mana = defaultMana;
        }
    }

    void RunEverySecond()
    {
        foreach(Card card in inventory)
        {
            if(card != null)
            {
                card.ApplyEffects(this);
            }         
        }
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
