using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loot : MonoBehaviour
{
    public static Loot S;

    public Deck initDeck;
    private List<Card> deck;

    public Transform drawPile;
    public Transform discardPile;
    public GameObject goldCoin;

    public List<Player> players;
    public AIPlayer AIPlayerLeft, AIPlayerTop, AIPlayerRight;
    public HumanPlayer humanPlayerPrefab;

    public int currentTurn;
    public bool waitingForPlayerInput;

    public GameObject battlePrefab;
    public GameObject battleUI;

    public Card cardToAddToBattle;
    public Card cardToDiscard;
    private bool waitingToDiscard;

    public GameObject cardHighlight;
    public GameObject PassTurnButton;

    public Text winTxt;
    public Text promptTxt;

    public Image timer;
    private float countDown;
    public float reloadDelay;

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

        players.Add(humanPlayerPrefab); //player 0
        players.Add(AIPlayerLeft); //player 1
        players.Add(AIPlayerTop); //player 2
        players.Add(AIPlayerRight); //player 3

        cardHighlight.SetActive(false);
        PassTurnButton.SetActive(false);

        winTxt.text = "";
        promptTxt.text = "";

        countDown = reloadDelay * 2;
        timer.gameObject.SetActive(false);

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

        if(countDown < reloadDelay)
        {
            countDown += Time.deltaTime;
            timer.fillAmount = (1 / reloadDelay) * countDown;
        }
    }

    Card Draw()
    {
        Card cd = deck[0];
        deck.RemoveAt(0);
        return cd;
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

            players[p].AddGold(0);
        }

        players[0].StartTurn();
    }

    public void AIDrawCard(int id)
    {
        if (deck.Count > 0)
        {
            players[id].drawCard(Draw());
        }
        else
        {
            players[id].GetComponent<AIPlayer>().DiscardLowestPirate();
        }

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

    public bool SelectedBattle(Battle battle)
    {
        if (waitingForPlayerInput)
        {
            if (battle.addToBattle(currentTurn, cardToAddToBattle))
            {
                print("added card to battle");
                battle.cardIntoBattle(players[currentTurn].discardCard(cardToAddToBattle));

                deselectCard();

                PassTurn();//Pass turn
                return true;
            }
            else
            {
                print("error, card invalid");
            }

            waitingForPlayerInput = false;
            deselectCard();
        }

        print("////// [No Player Input] ///////");

        return false;
    }

    //Discard pile/button clicked
    public void DiscardClicked()
    {
        waitingToDiscard = true;

        promptTxt.text = "Selecte a Pirate or Captain card to Discard";
    }

    void discardCard(Card cd)
    {
        if(players[currentTurn].discardCard(cd))
        {
            cd.transform.parent = discardPile;
            cd.transform.position = discardPile.position;
            cd.faceUp = true;
            cd.state = cardState.discard;

            promptTxt.text = "";
            waitingToDiscard = false;

            deselectCard();
            PassTurn();
        }
        else
        {
            print("[ERROR] cannot discard: card not found");

            promptTxt.text = "";
            deselectCard();
            waitingToDiscard = false;
        }
    }

    public void BattleClicked(Card cd)
    {
        waitingForPlayerInput = true;
        cardToAddToBattle = cd;

        players[currentTurn].WaitingForInput();
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
                if (waitingToDiscard == true)
                {
                    discardCard(cd);
                }
                else
                {
                    BattleClicked(cd);
                }
            }
            else if (cd.GetComponent<CaptainCard>())
            {
                if (waitingToDiscard == true)
                {
                    discardCard(cd);
                }
                else
                {
                    BattleClicked(cd);
                }
            }
        }
        else
        {
            print("not your card");
        }
    }

    public void humanPlayerPassedTurn()
    {
        if(currentTurn == 0)
        {
            PassTurn();
        }
    }

    void PassTurn()
    {
        if (CheckWinConditions())
        {
            int winningP = GetWinningPlayer();
            print("====++++==== GAME OVER Player[" + GetWinningPlayer() + "] WON ====++++====");

            winTxt.text = "GAME OVER \n Player[" + winningP + "] Won The Game! \n with " + players[winningP].goldCount + " Gold";
            countDown = 0;
            timer.gameObject.SetActive(true);

            Invoke("reloadScene", 3f);
        }
        else
        {
            if (currentTurn < players.Count - 1)
            {
                currentTurn++;
            }
            else
            {
                currentTurn = 0;
            }

            players[currentTurn].StartTurn();

            Battle[] battles = FindObjectsOfType<Battle>();

            for (int i = 0; i < battles.Length; i++)
            {
                battles[i].newTurn(currentTurn);
            }

            if(deck.Count <= 0)
            {
                PassTurnButton.SetActive(true);
            }

            print("///////======== New Turn ========////////");
        }
    }

    void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public Player getPlayerFromId(int id)
    {
        return players[id];
    }

    public void AwardGoldToID(int playerID, int amount, Vector3 shipPos)
    {
        GameObject thisCoin = Instantiate(goldCoin);
        thisCoin.transform.position = shipPos;

        thisCoin.GetComponent<CoinAnimation>().startMovement(players[playerID].transform);

        players[playerID].AddGold(amount);
    }

    bool CheckWinConditions()
    {
        bool end = false;

        if(deck.Count == 0)
        {
            for(int i = 0; i < players.Count; i++)
            {
                if(players[i].hand.Count == 0)
                {
                    end = true;
                }
            }
        }

        return end;
    }

    int GetWinningPlayer()
    {
        int winningID = -1;
        int goldCount = 0;

        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].goldCount > goldCount)
            {
                winningID = i;
                goldCount = players[i].goldCount;
            }
        }

        return winningID;
    }
}