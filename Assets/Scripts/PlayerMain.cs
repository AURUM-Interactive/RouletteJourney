using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMain : MonoBehaviour
{
    public int health, maxHP;
    public float HPRegenPerSecond = 1;
    public int mana, maxMana;
    public float ManaRegenPerSecond = 1;

    private float tempHP = 0;
    private float tempMana = 0;

    private Slider HPBar, ManaBar;
    private TextMeshProUGUI HPCounter, ManaCounter;

    // TODO: implement inventory here 
    //public List<Card> Inventory;


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



    void Regenerate()
    {
        if(health < maxHP)
        {
            tempHP += ProjUtilities.SecondsToFixed(HPRegenPerSecond);
            if(tempHP >= 1){ 
                health += 1;
                tempHP = 0;
            }}
        if(mana < maxMana){
            tempMana += ProjUtilities.SecondsToFixed(ManaRegenPerSecond);
            if(tempMana >= 1){ 
                mana += 1;
                tempMana = 0;
            }
        }  
    }
}
