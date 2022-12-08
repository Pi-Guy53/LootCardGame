using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    public bool topOfBoard;

    private Battle[] allBattles;

    private List<PirateCard> pirates;
    private List<MerchantCard> merchants;
    private List<CaptainCard> captains;

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

        allBattles = FindObjectsOfType<Battle>();
        fillLists();
        tempGoldCount = 0;

        if (allBattles.Length > 0)
        {
            for(int i = 0; i < allBattles.Length; i++)
            {
                if(allBattles[i].winningPlayerColor() == playerID)
                {
                    //skip over
                }
                else
                {
                    if(allBattles[i].goldValue > tempGoldCount)
                    {
                        tempGoldCount = allBattles[i].goldValue;
                    }
                }
            }
        }
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

    public override void WaitingForInput()
    {
        //choose battle to join
    }

    public override Vector3 HomeWaters()
    {
        Vector3 homeWaterPos = Vector3.zero;

        if (topOfBoard)
        {
            homeWaterPos = homeWaters.transform.position + (Vector3.right * (5 - battles.Count));
        }
        else
        {
            if (battles.Count% 2 == 0)
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

        return homeWaterPos;
    }

    public override void DisplayHand()
    {
        //Display face down hand
        for(int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.position = handAnchor.transform.position + handAnchor.transform.right * ((i * .25f) - hand.Count / 2);
            hand[i].sortingOrder = i;
            hand[i].faceUp = false;
        }
    }

    void AskForCard()
    {
        Loot.S.AIDrawCard(playerID);
    }

    void PlayMerchant()
    {
        foreach(Card cd in hand)
        {
            if (cd.GetComponent<MerchantCard>())
            {
                Loot.S.AICardClicked(cd);
                return;
            }
        }
        //Loot.S.AICardClicked( /*Choose Pirate From Hand*/ );
    }

    void JoinBattle()
    {
        //Loot.S.BattleClicked( /*Choose Pirate From Hand*/ );
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

}