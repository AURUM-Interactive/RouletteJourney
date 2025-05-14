using UnityEngine;

public class GamblingScreen : MonoBehaviour
{

    public CardData[] inventory = new CardData[3];
    public int money = -1;
    public int gambleCost = 5;

    public GameObject cardPrefab;
    public GameObject discardPrefab;

    public GameObject Slot1;
    public GameObject Slot2;
    public GameObject Slot3;

    private GameObject Interface; //For creating the buttons and assigning them to the interface

    public void DisplayCards()
    {
        // Array of slot transforms
        Transform[] slotTransforms = { Slot1.transform, Slot2.transform, Slot3.transform };

        // Array of discard offset vectors relative to each slot
        Vector3[] discardOffsets = new Vector3[3];
        discardOffsets[0] = new Vector3(Slot1.transform.localPosition.x - 195, Slot1.transform.localPosition.y + 500, 0);
        discardOffsets[1] = new Vector3(Slot2.transform.localPosition.x, Slot2.transform.localPosition.y + 500, 0);
        discardOffsets[2] = new Vector3(Slot3.transform.localPosition.x + 195, Slot3.transform.localPosition.y + 500, 0);

        // Loop through the first 3 inventory slots
        for (int i = 0; i < 3; i++)
        {
            // Only process non-null inventory and if the corresponding slot is empty
            if (inventory[i] != null && slotTransforms[i].childCount == 0)
            {
                CardData card = inventory[i];
                // Instantiate the card prefab in the proper slot
                GameObject createdCard = Instantiate(cardPrefab, slotTransforms[i]);

                // Instantiate and position the discard card prefab
                GameObject cardDiscard = Instantiate(discardPrefab, Interface.transform);
                cardDiscard.transform.Translate(discardOffsets[i]);
                cardDiscard.GetComponent<DiscardScript>().cardToDiscard = createdCard;

                // Initialize the card's GUI based on whether it's consumable
                CardGUI cardGUI = createdCard.GetComponent<CardGUI>();
                if (card.consumable)
                {
                    cardGUI.Initialize(card, card.CardName, card.CardDescription, card.HealAmount, card.ManaAmount);
                }
                else
                {
                    cardGUI.Initialize(card, card.CardName, card.CardDescription, card.HPRegenChange, card.manaRegenChange, card.maxHPChange, card.maxManaChange);
                }
                cardGUI.consumable = false;
            }
        }
    }

    void Start()
    {

        Slot1 = GameObject.Find("Card1Pos");
        Slot2 = GameObject.Find("Card2Pos");
        Slot3 = GameObject.Find("Card3Pos");

        Interface = this.gameObject;

        inventory.SetValue(new CardData("Example Consumable", "Use me, I heal", 10, 0), 0);
        inventory.SetValue(new CardData("Example Passive", "+1 HP per second", 1, 0, 0, 0), 1);

        //Takes cards from inventory array and displays them on screen
        DisplayCards();
        
    }

    public void GambleSlot1()
    {
        Gamble(Slot1);
    }
    public void GambleSlot2()
    {
        Gamble(Slot2);
    }
    public void GambleSlot3()
    {
        Gamble(Slot3);
    }

    void Gamble(GameObject Slot)
    {
        if( money >= gambleCost )
        {
            money -= gambleCost;
            CardData NewCard = null;

            NewCard = new CardData("RandomCardName", "RandomDescription", 1, 2);

            if (Slot == Slot1)
            {
                inventory.SetValue(NewCard, 0);
            }
            if (Slot == Slot2)
            {
                inventory.SetValue(NewCard, 1);
            }
            if (Slot == Slot3)
            {
                inventory.SetValue(NewCard, 2);
            }

            DisplayCards();
        }
        //TODO show the user that funds are insufficient

    }

}
