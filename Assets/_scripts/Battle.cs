using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public int goldValue;

    public ColorToPlayer blue;
    public ColorToPlayer green;
    public ColorToPlayer yellow;
    public ColorToPlayer purple;

    public int turnIDOfBattleEnd;

    private void Start()
    {
        blue.shipColor = cardColor.blue;
        green.shipColor = cardColor.green;
        yellow.shipColor = cardColor.yellow;
        purple.shipColor = cardColor.purple;
    }

    public void setUp(int currentTurn, int gv)
    {
        goldValue = gv;
        turnIDOfBattleEnd = currentTurn;
    }

    public void newTurn(int currentTurn)
    {
        if (currentTurn == turnIDOfBattleEnd)
        {
            //check if there are no ties
        }
    }

    public bool addToBattle(int playerID, Card cd)
    {
        return false;
    }
}