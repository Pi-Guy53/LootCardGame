using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loot : MonoBehaviour
{
    public static Loot S;

    public Deck initDeck;
    private List<Card> deck;

    public Transform drawPile;
    public Transform discardPile;

    public List<Player> players;
    public AIPlayer AIPlayerPrefab;
    public HumanPlayer humanPlayerPrefab;

    public int currentTurn;
    private bool waitingForPlayerInput;

    public GameObject battlePrefab;
    public GameObject battleUI;

    private Card cardToAddToBattle;

    public GameObject cardHighlight;

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
        players.Add(AIPlayerPrefab);

        cardHighlight.SetActive(false);

        startGame();
    }

    private void Update()
    {
        if (cardToAddToBattle != null)
        {
            cardHighlight.SetActive(true);
            cardHighlight.transform.position = cardToAddToBattle.transform.position;
        }
        else
        {
            cardHighlight.SetActive(false);
        }
    }

    Card Draw()
    {
        Card cd = deck[0];
        deck.RemoveAt(0);
        return cd;
    }

    void discardCard(Card cd)
    {
        cd.transform.parent = discardPile;
        cd.transform.position = discardPile.position;
        cd.faceUp = true;
        cd.state = cardState.discard;
    }

    public void startGame()
    {
        for (int p = 0; p < players.Count; p++)
        {
            players[p].playerID = p;

            for (int i = 0; i < 6; i++)
            {
                players[p].drawCard(Draw());
            }
        }
    }

    public void AIDrawCard(int id)
    {
        players[id].drawCard(Draw());

        PassTurn();//Pass turn
    }

    public void AICardClicked(Card cd)
    {
        switch (cd.state)
        {
            case cardState.hand:
                playCardFromHand(cd);
                break;

            case cardState.deck:
            case cardState.discard:
            case cardState.battle:
                break;
        }
    }

    public void CardClicked(Card cd, bool isHumanPlayer)
    {
        if (isHumanPlayer && currentTurn == 0)
        {
            switch (cd.state)
            {
                case cardState.deck:
                    players[currentTurn].drawCard(Draw());
                    PassTurn();//Pass turn

                    deselectCard();
                    break;

                case cardState.hand:
                    deselectCard();
                    playCardFromHand(cd);
                    break;

                case cardState.discard:
                case cardState.battle:
                    deselectCard();
                    break;
            }
        }
        else if (isHumanPlayer)
        {
            print("Not your turn");
        }
        else
        {
            print("AI Selected action: Should not have happened");
        }
    }

    void deselectCard()
    {
        waitingForPlayerInput = false;
        cardToAddToBattle = null;
    }

    public void SelectedBattle(Battle battle)
    {
        if (waitingForPlayerInput)
        {
            if(battle.addToBattle(currentTurn, cardToAddToBattle))
            {
                print("added card to battle");
                battle.cardIntoBattle(players[currentTurn].discardCard(cardToAddToBattle));

                deselectCard();

                PassTurn();//Pass turn
            }
            else
            {
                print("error, card invalid");
            }

            waitingForPlayerInput = false;
            deselectCard();
        }
    }

    public void BattleClicked(Card cd)
    {
        players[currentTurn].WaitingForInput();
        waitingForPlayerInput = true;
        cardToAddToBattle = cd;
    }

    void createBattle(Card cd)
    {
        GameObject newBattle = Instantiate(battlePrefab);
        newBattle.GetComponent<Battle>().setUp(currentTurn, cd.GetComponent<MerchantCard>().goldValue, currentTurn);

        players[currentTurn].addBattle(newBattle.GetComponent<Battle>());
        newBattle.transform.position = players[currentTurn].HomeWaters();

        BattleUI newBUI = Instantiate(battleUI).GetComponent<BattleUI>();

        newBUI.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        newBUI.transform.position = Camera.main.WorldToScreenPoint(newBattle.transform.position);
        newBUI.goldTxt.text = "$" + cd.GetComponent<MerchantCard>().goldValue;

        newBattle.GetComponent<Battle>().addUI(newBUI);

        cd.transform.localScale = Vector3.one;
        cd.state = cardState.battle;
        cd.hover = false;
        cd.transform.position = newBattle.transform.position;
        cd.transform.parent = newBattle.transform;

        players[currentTurn].discardCard(cd);

        PassTurn();//Pass turn
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
                BattleClicked(cd);
            }
            else if (cd.GetComponent<CaptainCard>())
            {
                BattleClicked(cd);
            }
        }
        else
        {
            print("not your card");
        }
    }

    void PassTurn()
    {
        if (currentTurn < players.Count -1)
        {
            currentTurn++;
        }
        else
        {
            currentTurn = 0;
        }

        players[currentTurn].StartTurn();

        Battle[] battles = FindObjectsOfType<Battle>();

        for(int i = 0; i < battles.Length; i++)
        {
            battles[i].newTurn(currentTurn);
        }
    }

    public Player getPlayerFromId(int id)
    {
        return players[id];
    }

}