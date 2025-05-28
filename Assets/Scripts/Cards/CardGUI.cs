using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.GPUSort;

public class CardGUI : MonoBehaviour
{
    public string CardName = "";
    public string CardDescription = "";

    private CardData inventoryInstance = null;

    [Header("Passive Card Effects")]
    public float HPRegenChange;
    public float manaRegenChange;
    public int maxHPChange, maxManaChange;

    [Header("Consumable Effects")]
    public bool consumable = false;
    public bool consumed = false;

    public int HealAmount;
    public int ManaAmount;

    /// <summary>
    /// Initializer for passive card
    /// </summary>
    /// <param name="name"> Card Name </param>
    /// <param name="description">Description (effects) </param>
    /// <param name="hpregen"> HP regeneration speed change </param>
    /// <param name="manaregen"> Mana regeneration speed change </param>
    /// <param name="maxhp"> Maximum HP increase </param>
    /// <param name="maxmana"> Maximum mana increase </param>
    public void Initialize(CardData inventoryCard, string name, string description, float hpregen, float manaregen, int maxhp, int maxmana)
    {
        CardName = name; 
        CardDescription = description;
        this.GetComponent<Image>().sprite = inventoryCard.cardSprite;
        inventoryInstance = inventoryCard;
        consumable = false;
        HPRegenChange = hpregen;
        manaRegenChange = manaregen;
        maxHPChange = maxhp;
        maxManaChange = maxmana;
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
    public CardGUI(string name, string description, float hpregen, float manaregen, int maxhp, int maxmana) 
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
    /// Initializer for consumable card
    /// </summary>
    /// <param name="cardName"> Card Name </param>
    /// <param name="cardDescription"> Description (effects) </param>
    /// <param name="healAmount"> Heal amount on use </param>
    /// <param name="manaAmount"> Mana amount on use </param>
    public void Initialize(CardData inventoryCard, string cardName, string cardDescription, int healAmount, int manaAmount)
    {
        CardName = cardName;
        CardDescription = cardDescription;
        this.GetComponent<Image>().sprite = inventoryCard.cardSprite;
        inventoryInstance = inventoryCard;
        consumable = true;
        HealAmount = healAmount;
        ManaAmount = manaAmount;
    }

    /// <summary>
    /// Constructor for consumable card
    /// </summary>
    /// <param name="cardName"> Card Name </param>
    /// <param name="cardDescription"> Description (effects) </param>
    /// <param name="healAmount"> Heal amount on use </param>
    /// <param name="manaAmount"> Mana amount on use </param>
    public CardGUI(string cardName, string cardDescription, int healAmount, int manaAmount)
    {
        CardName = cardName;
        CardDescription = cardDescription;
        consumable = true;
        HealAmount = healAmount;
        ManaAmount = manaAmount;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextMeshProUGUI CardTitleText = this.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI CardDesc = this.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        GameObject useButton = this.transform.GetChild(2).GameObject();
        

        CardTitleText.text = CardName;
        CardDesc.text = CardDescription;

        if(!consumable)
        {
            useButton.SetActive(false);
        }
    }

    public void UseCard()
    {
        PlayerMain player = GameObject.Find("Player").GetComponent<PlayerMain>();
        player.health += HealAmount;
        player.mana += ManaAmount;
        inventoryInstance.consumed = true;         
        Destroy(this.gameObject);
    }

    public void ApplyEffects(PlayerMain player)
    {
        player.HPRegenPerSecond += HPRegenChange;
        player.ManaRegenPerSecond += manaRegenChange;
        player.maxHP += maxHPChange;
        player.maxMana += maxManaChange;
    }

    public void DiscardThis()
    {
        GamblingScreen GS = GameObject.Find("UserInterface").GetComponent<GamblingScreen>();
        for(int i = 0; i < 3; i++)
        {
            if (GS.inventory[i] == inventoryInstance)
            {
                GS.inventory.SetValue(null, i);
                Debug.Log("Card removed at " + i);
            }
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
