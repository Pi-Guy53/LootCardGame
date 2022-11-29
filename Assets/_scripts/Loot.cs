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

    public int currentTurn;
    private bool waitingForPlayerInput;

    public GameObject battlePrefab;
    public GameObject battleUI;

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

    public void AICardClicked(Card cd)
    {

    }

    public void CardClicked(Card cd, bool isHumanPlayer)
    {
        if (isHumanPlayer && currentTurn == 0)
        {
            switch (cd.state)
            {
                case cardState.deck:
                    players[currentTurn].drawCard(Draw());
                    break;

                case cardState.hand:
                    playCardFromHand(cd);
                    break;

                case cardState.discard:
                case cardState.battle:
                    break;
            }
        }
        else if(isHumanPlayer)
        {
            print("Not your turn");
        }
        else
        {
            print("AI Selected action: Should not have happened");
        }
    }

    public void BattleClicked()
    {
        players[currentTurn].WaitingForInput();
    }

    void createBattle(Card cd)
    {
        GameObject newBattle = Instantiate(battlePrefab);
        newBattle.GetComponent<Battle>().setUp(currentTurn, cd.GetComponent<MerchantCard>().goldValue);
        newBattle.transform.position = players[currentTurn].HomeWaters();

        GameObject newBUI = Instantiate(battleUI);
        newBUI.transform.parent = GameObject.FindGameObjectWithTag("UI").transform;
        newBUI.transform.position = Camera.main.WorldToScreenPoint(newBattle.transform.position);
    }

    void playCardFromHand(Card cd)
    {
        if (players[currentTurn].containsCard(cd))
        {
            if (cd.GetComponent<MerchantCard>())
            {
                createBattle(cd);
            }
            else if (cd.GetComponent<PirateCard>())
            {
                BattleClicked();
            }
            else if (cd.GetComponent<CaptainCard>())
            {
                BattleClicked();
            }
        }
        else
        {
            print("not your card");
        }
    }

}