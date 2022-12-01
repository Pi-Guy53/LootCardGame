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

    public int winningPlayer;

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

    private ColorToPlayer getColorToModify(cardColor col)
    {
        switch (col)
        {
            case cardColor.green:
                return green;


            case cardColor.blue:
                return blue;


            case cardColor.yellow:
                return yellow;


            case cardColor.purple:
                return purple;

            default:
                return null;
        }
    }

    bool checkPlayerIds(int playerID)
    {
        //check if the player has already played a card to a different color

        return false;
    }

    bool checkPlayerColor(int pID, cardColor color)
    {
        //allows a player to only add to an existing color
        if (color == cardColor.blue)
        {
            if (blue.hasID() && blue.checkID(pID))
            {
                return true;
            }
            else
            {
                return false;
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
                return false;
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
                return false;
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
                return false;
            }
        }
        else if (color == cardColor.admiral)
        {
            if (battleOwner == pID)
            {
                winningPlayer = pID;
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
        cardColor col = cardColor.none;

        if (cd.GetComponent<PirateCard>())
        {
            col = cd.GetComponent<PirateCard>().color;

            if (checkPlayerColor(playerID, col))
            {
                getColorToModify(col).strength += cd.GetComponent<PirateCard>().strength;
                getColorToModify(col).playerID = playerID;
            }

            return true;
        }
        else if (cd.GetComponent<CaptainCard>())
        {
            col = cd.GetComponent<CaptainCard>().color;

            winningPlayer = playerID;

            return true;
        }

        return false;
    }

    public void AIClicked()
    {
        Loot.S.SelectedBattle(this);
    }

    public void OnMouseUpAsButton()
    {
        Loot.S.SelectedBattle(this);
    }
}