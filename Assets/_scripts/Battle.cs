using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public int goldValue;
    public int turnIDOfBattleEnd;
    public int battleOwner;

    public ColorToPlayer blue;
    public ColorToPlayer green;
    public ColorToPlayer yellow;
    public ColorToPlayer purple;

    private void Start()
    {
        blue.setColor(cardColor.blue);
        green.setColor(cardColor.green);
        yellow.setColor(cardColor.yellow);
        purple.setColor(cardColor.purple);
    }

    public void setUp(int currentTurn, int gv, int owner)
    {
        goldValue = gv;
        turnIDOfBattleEnd = currentTurn;
        battleOwner = owner;
    }

    public void newTurn(int currentTurn)
    {
        if (currentTurn == turnIDOfBattleEnd)
        {
            //check if there are no ties
        }
    }

    bool checkPlayerColor(int pID, cardColor color)
    {
        if (color == cardColor.blue)
        {
            if (blue.hasID() && blue.checkID(pID))
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        else if (color == cardColor.green)
        {
            if (green.hasID() && green.checkID(pID))
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        else if (color == cardColor.yellow)
        {
            if (yellow.hasID() && yellow.checkID(pID))
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        else if (color == cardColor.purple)
        {
            if (purple.hasID() && purple.checkID(pID))
            {
                return true;
            }
            else
            {
                return true;
            }
        }
        else if (color == cardColor.admiral)
        {
            if (battleOwner == pID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool addToBattle(int playerID, Card cd)
    {


        return false;
    }
}