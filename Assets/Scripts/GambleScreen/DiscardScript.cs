using UnityEngine;

public class DiscardScript : MonoBehaviour
{
    public GameObject cardToDiscard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Discard()
    {
        cardToDiscard.GetComponent<CardGUI>().DiscardThis();
        Debug.Log("discarding " + cardToDiscard.name);
        GamblingScreen GS = GameObject.Find("UserInterface").GetComponent<GamblingScreen>();
        GS.DisplayCards();
        Destroy(this.gameObject);
    }
}
