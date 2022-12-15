using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    public bool topOfBoard;

    private Battle[] allBattlesArray;

    private List<PirateCard> pirates;
    private List<MerchantCard> merchants;
    private List<CaptainCard> captains;

    private List<Battle> allBattles;

    int tempGoldCount;

    private void Start()
    {
        
    }

    public override void StartTurn()
    {
        //Invoke("AskForCard", .5f); //delay for "thinking"
        //Invoke("PlayMerchant", .5f); //delay for "thinking"

        //take an action
        //check all battles;
        //see if it should continue a fight:
        //see if it should join a fight:
        //see if to deply a merchant:
        //draw a card

        fillLists();

        allBattlesArray = FindObjectsOfType<Battle>();
        allBattles = new List<Battle>();

        for(int i = 0; i < allBattlesArray.Length; i++)
        {
            allBattles.Add(allBattlesArray[i]);
        }

        Invoke("PlayPirateFromHand", .3333f);
    }

    void fillLists()
    {
        pirates = new List<PirateCard>();
        merchants = new List<MerchantCard>();
        captains = new List<CaptainCard>();

        for (int i = 0; i < hand.Count; i++)
        {
            if(hand[i].GetComponent<PirateCard>())
            {
                pirates.Add(hand[i].GetComponent<PirateCard>());
            }
            else if(hand[i].GetComponent<CaptainCard>())
            {
                captains.Add(hand[i].GetComponent<CaptainCard>());
            }
            else if(hand[i].GetComponent<MerchantCard>())
            {
                merchants.Add(hand[i].GetComponent<MerchantCard>());
            }
        }
    }

    private bool handHas(int cardType)
    {
        if (cardType == 1) //pirate
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].GetComponent<PirateCard>())
                {
                    return true;
                }
            }
        }
        else if (cardType == 2) //captain
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].GetComponent<CaptainCard>())
                {
                    return true;
                }
            }
        }
        else if (cardType == 3) //merchant
        {
            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i].GetComponent<MerchantCard>())
                {
                    return true;
                }
            }
        }

        return false;
    }

    void PlayPirateFromHand()
    {
        print("playing pirate");

        foreach (Card cd in hand)
        {
            if (cd.GetComponent<PirateCard>() || cd.GetComponent<CaptainCard>())
            {
                Loot.S.AICardClicked(cd);
                return;
            }
        }

        noPirateToPlay();
    }

    public override void WaitingForInput()
    {
        print("waiting for input");

        Battle b = chooseBattle();

        if (b != null)
        {
            print("not null");

            /*
            if (b.checkPlayerIds(playerID) == 1)
            {
                print("continuing a battle");

                if (Loot.S.SelectedBattle(b))
                {
                    return;
                }
            }
            */

            print("++++ choose a pirate to join a new battle $" + b.goldValue);
            print(pirates.Count + " pirates avalible");

            for (int i = 0; i < pirates.Count; i++)
            {
                bool colorFree = b.isColorFree(pirates[i].color, playerID);

                print(colorFree);

                if (colorFree)
                {
                    print(pirates[i].color + " : " + b.checkPlayerColor(playerID, pirates[i].color) + " : " + b.getColorToModify(pirates[i].color).playerID);

                    Loot.S.waitingForPlayerInput = true;
                    Loot.S.cardToAddToBattle = pirates[i];

                    if (Loot.S.SelectedBattle(b))
                    {
                        print("joining a battle");

                        return;
                    }
                    else
                    {
                        //continue
                    }
                }
            }

        }

        allBattles.Remove(b);

        couldNotJoinHighestBattle();
    }

    void couldNotJoinHighestBattle()
    {
        print("could not join highest battle");

        PlayMerchantFromHand();
    }

    void noPirateToPlay()
    {
        print("could not play pirate");

        PlayMerchantFromHand();
    }

    void PlayMerchantFromHand()
    {
        print("playing merchant");

        foreach (Card cd in merchants)
        {
            Loot.S.AICardClicked(cd);
            return;
        }

        noMerhcantToPlay();
    }

    void noMerhcantToPlay()
    {
        print("could not play merchant");

        AskForCard();
    }

    void AskForCard()
    {
        print("drawing card");

        Loot.S.AIDrawCard(playerID);
    }

    Battle chooseBattle()
    {
        print("chooseing battle");

        tempGoldCount = 0;
        Battle highestValueBattle = null;

        if (allBattles.Count > 0)
        {
            for (int i = 0; i < allBattles.Count; i++)
            {
                if ((allBattles[i].winningPlayerColor() == playerID || allBattles[i].winningOwner(playerID)) && !allBattles[i].TiedStrength())
                {
                    //skip over
                }
                else
                {
                    if (allBattles[i].goldValue > tempGoldCount)
                    {
                        tempGoldCount = allBattles[i].goldValue;

                        highestValueBattle = allBattles[i];
                    }
                }
            }
        }

        return highestValueBattle;
    }

    public void DiscardLowestPirate()
    {
        if (discardCard(chooseLowestPirateCard()))
        {
            print("discarded lowest card");
        }
    }

    Card chooseLowestPirateCard()
    {
        int tempStr = 6;
        Card toReturn = null;

        if (pirates.Count > 1)
        {
            for (int i = 0; i < pirates.Count; i++)
            {
                if (pirates[i].strength < tempStr)
                {
                    tempStr = pirates[i].strength;
                    toReturn = pirates[i];
                }
            }
        }
        else if(captains.Count > 0)
        {
            toReturn = captains[0];
        }

        return toReturn;
    }
    /**
     * Actions: 
     * Draw Card (no card limit)
     * Play a Merchant
     * Play a pirtate to attack a merchant ship
     * Strengthen an existing Attack
     * Play a Captain to strengthen an existing Attack
     * Play the Admiral to defend your own Merchant
     */

    public override Vector3 HomeWaters()
    {
        Vector3 homeWaterPos = Vector3.zero;

        if (topOfBoard)
        {
            homeWaterPos = homeWaters.transform.position + (Vector3.right * (5 - battles.Count) * 3);
        }
        else
        {
            if (battles.Count % 2 == 0)
            {
                homeWaterPos.x = homeWaters.transform.position.x;
                homeWaterPos.y = (homeWaters.transform.position.y) + (battles.Count - 1) * 2.25f;
            }
            else
            {
                homeWaterPos.x = homeWaters.transform.position.x + 4;
                homeWaterPos.y = (homeWaters.transform.position.y) + (battles.Count) * 2.25f;
            }
        }

        for (int i = 0; i < battles.Count; i++)
        {
            if (homeWaterPos == battles[i].transform.position)
            {
                int battleC = battles.Count + 1;

                if (topOfBoard)
                {
                    homeWaterPos = homeWaters.transform.position + (Vector3.right * (5 - battleC) * 3);
                }
                else
                {
                    if (battleC % 2 == 0)
                    {
                        homeWaterPos.x = homeWaters.transform.position.x;
                        homeWaterPos.y = (homeWaters.transform.position.y) + (battleC - 1) * 2.25f;
                    }
                    else
                    {
                        homeWaterPos.x = homeWaters.transform.position.x + 4;
                        homeWaterPos.y = (homeWaters.transform.position.y) + (battleC) * 2.25f;
                    }
                }
            }
        }

        return homeWaterPos;
    }

    public override void DisplayHand()
    {
        //Display face down hand
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.position = handAnchor.transform.position + handAnchor.transform.right * ((i * .25f) - hand.Count / 2);
            hand[i].sortingOrder = i;
            hand[i].faceUp = false;
        }
    }

}