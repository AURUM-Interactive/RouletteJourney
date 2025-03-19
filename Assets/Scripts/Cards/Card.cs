using UnityEngine;

public class Card : MonoBehaviour
{
    public readonly float HPRegenChange, manaRegenChange;
    public readonly int maxHPChange, maxManaChange;
    public readonly string CardName = ""; //TODO: Implement a way to display these
    public readonly string CardDescription = "";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
