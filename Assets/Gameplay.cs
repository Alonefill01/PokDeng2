using NUnit.Framework;
using NUnit.Framework.Internal.Builders;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Card
{
    public enum Suit {Hearts, Diamonds, Clubs, Spades}
    public enum Rank { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

    public Suit suit;
    public Rank rank;
    public Sprite cardSprite;

    public Card(Suit s, Rank r) 
    {
        suit = s;
        rank = r;
    }
    public int GetPokDengValue()
    {
        if (rank >= Rank.Jack) return 0; //J, Q, K = 0;
        if (rank == Rank.Ace) return 1;
        return (int)rank; // 2-10 = 2-10
    }

    public string GetDisplayName()
    {
        return rank.ToString() +  " of " + suit.ToString();
    }
}

public class Gameplay : MonoBehaviour
{
    [Header("UI References")]
    public Button butDeal;
    public Image[] imgCard = new Image[4];
    public TextMeshProUGUI txtPlayerScore, txtAIScore, txtResult;

    [Header("Card Sprites")]
    public Sprite[] cardSprites = new Sprite[52];
    public Sprite cardBackSprite;

    private List<Card> deck = new List<Card>();
    private List<Card> playerHand = new List<Card>();
    private List<Card> aiHand = new List<Card>();

    int playerScore, aiScore;


    void Start()
    {
        butDeal.onClick.AddListener(DealCards);
    }

    void CreateDeck()
    {
        deck.Clear();
        for (int suit = 0; suit <4; suit++)
        {
            for(int rank = 1; rank <= 13; rank++)
            {
                Card newCard = new Card((Card.Suit)suit, (Card.Rank)rank);
                int spriteIndex = suit * 13 + (rank - 1);
                if (spriteIndex < cardSprites.Length && cardSprites[ spriteIndex] != null)
                {
                    newCard.cardSprite = cardSprites[spriteIndex];
                }
                deck.Add(newCard);

            }
        }
    }
    void ShuffleDeck()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Card temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    void ClearHand()
    {
        playerHand.Clear();
        aiHand.Clear();
        txtResult.text = "";    
    }

    void DealCards()
    {
        ClearHand();
        for (int i = 0; i < 2; i++)
        {
            playerHand.Add(DrawCard());
            aiHand.Add(DrawCard());
        }
        UpdateImages();
        UpdateScore();
        CheckResult();
    }

    
    Card DrawCard()
    {
        if (deck.Count == 0)
        {
            CreateDeck();
            ShuffleDeck();
        }
        Card drawnCard = deck[0];
        deck.RemoveAt(0);
        return drawnCard;
    }

   void UpdateImages()
    {
        for (int i=0; i <2; i++)
        {
            imgCard[i].sprite = playerHand[i].cardSprite;
            imgCard[i + 2].sprite = aiHand[i].cardSprite;
        }
    }

    void UpdateScore()
    {
        playerScore = playerHand[0].GetPokDengValue() + playerHand[1].GetPokDengValue();
        aiScore = aiHand[0].GetPokDengValue() + aiHand[1].GetPokDengValue();
        if (playerScore > 9) playerScore -= 10;
        if (aiScore > 9) aiScore -= 10;
        txtPlayerScore.text = playerScore.ToString();
        txtAIScore.text = aiScore.ToString();   
    }
    
    void CheckResult()
    {
        if (playerScore > aiScore)
        {
            txtResult.text = "Player Wins!";
        }
        else if (playerScore < aiScore)
        {
            txtResult.text = "AI Wins!";
        }
        else txtResult.text = "It's a Draw!";

    }
}
