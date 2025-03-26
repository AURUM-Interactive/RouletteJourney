using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CardData
{
    public string CardName = "";
    public string CardDescription = "";

    private CardGUI inventoryInstance = null;

    [Header("Passive Card Effects")]
    public float HPRegenChange;
    public float manaRegenChange;
    public int maxHPChange, maxManaChange;

    [Header("Consumable Effects")]
    public bool consumable = false;
    public bool consumed = false;

    public int HealAmount;
    public int ManaAmount;

    public void ApplyEffects(PlayerMain player)
    {
        player.HPRegenPerSecond += HPRegenChange;
        player.ManaRegenPerSecond += manaRegenChange;
        player.maxHP += maxHPChange;
        player.maxMana += maxManaChange;
    }

    /// <summary>
    /// Constructor for passive card
    /// </summary>
    /// <param name="name"> Card Name </param>
    /// <param name="description">Description (effects) </param>
    /// <param name="hpregen"> HP regeneration speed change </param>
    /// <param name="manaregen"> Mana regeneration speed change </param>
    /// <param name="maxhp"> Maximum HP increase </param>
    /// <param name="maxmana"> Maximum mana increase </param>
    public CardData(string name, string description, float hpregen, float manaregen, int maxhp, int maxmana) 
    {
        CardName = name;
        CardDescription = description;
        consumable = false;
        HPRegenChange = hpregen;
        manaRegenChange = manaregen;
        maxHPChange = maxhp;
        maxManaChange = maxmana;
    }


    /// <summary>
    /// Constructor for consumable card
    /// </summary>
    /// <param name="cardName"> Card Name </param>
    /// <param name="cardDescription"> Description (effects) </param>
    /// <param name="healAmount"> Heal amount on use </param>
    /// <param name="manaAmount"> Mana amount on use </param>
    public CardData(string cardName, string cardDescription, int healAmount, int manaAmount)
    {
        CardName = cardName;
        CardDescription = cardDescription;
        consumable = true;
        HealAmount = healAmount;
        ManaAmount = manaAmount;
    }

}
