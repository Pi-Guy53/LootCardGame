using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : Player
{
    private List<Battle> allBattles;

    private void Start()
    {
        allBattles = new List<Battle>();
    }

    public override void StartTurn()
    {
        //take an action
    }

    public override void WaitingForInput()
    {
        //choose battle to join
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