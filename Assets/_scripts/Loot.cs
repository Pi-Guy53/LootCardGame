using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public static Loot S;

    public Deck initDeck;
    private List<Card> deck;

    public Transform drawPile;
    public Transform discardPile;

    public List<Player> players;
    public AIPlayer AIPlayerPrefab;
    public Player humanPlayerPrefab;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        initDeck = GetComponent<Deck>();
        players = new List<Player>();

        initDeck.CreateDeck();
        deck = initDeck.GetDeck();

        initDeck.MoveDeck(drawPile.position);

        players.Add(humanPlayerPrefab);

        startGame();
    }

    Card Draw()
    {
        Card cd = deck[0];
        deck.RemoveAt(0);
        return cd;
    }

    public void startGame()
    {
        for (int i = 0; i < 5; i++)
        {
            players[0].drawCard(Draw());
        }
    }

    public void CardClicked(Card cd, bool isPlayer)
    {

        switch (cd.state)
        {
            case cardState.deck:
                break;

            case cardState.hand:
                break;

            case cardState.discard:
                break;

            case cardState.battle:
                break;
        }

        print(cd.name);
    }

}