using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{

    public bool topOfBoard;

    private List<Battle> allBattles;

    private void Start()
    {
        allBattles = new List<Battle>();
    }

    public override void StartTurn()
    {
        //take an action
        //Invoke("AskForCard", .5f); //delay for "thinking"
        Invoke("PlayMerchant", .5f); //delay for "thinking"
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