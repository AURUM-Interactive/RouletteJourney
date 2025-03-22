using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public string CardName = "";
    public string CardDescription = "";

    public TextMeshProUGUI CardTitleText;
    public TextMeshProUGUI CardDescText;
    
    public GameObject useButton;

    [Header("Passive Card Effects")]
    public float HPRegenChange;
    public float manaRegenChange;
    public int maxHPChange, maxManaChange;

    [Header("Consumable Effects")]
    public bool consumable = false;

    public int HealAmount;
    public int ManaAmount;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CardTitleText.text = CardName;
        CardDescText.text = CardDescription;
        if(!consumable)
        {
            useButton.SetActive(false);
        }
    }

    public void UseCard(PlayerMain player)
    {
        player.health += HealAmount;
        player.mana += ManaAmount;
        RemoveSelf();
    }

    public void RemoveSelf()
    {
        Destroy(this.gameObject);
    }

    public void ApplyEffects(PlayerMain player)
    {
        player.HPRegenPerSecond += HPRegenChange;
        player.ManaRegenPerSecond += manaRegenChange;
        player.maxHP += maxHPChange;
        player.maxMana += maxManaChange;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
