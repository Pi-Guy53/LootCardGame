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

    private int cardsInBattle;
    private BattleUI ui;

    public GameObject playerHighlight;

    private void Start()
    {
        blue.setColor(cardColor.blue);
        green.setColor(cardColor.green);
        yellow.setColor(cardColor.yellow);
        purple.setColor(cardColor.purple);

        playerHighlight.SetActive(false);
    }

    public void setUp(int currentTurn, int gv, int owner)
    {
        goldValue = gv;
        turnIDOfBattleEnd = currentTurn;
        battleOwner = owner;

        cardsInBattle = 0;
        winningPlayer = -1;
    }

    public void newTurn(int currentTurn)
    {
        print(currentTurn + " : " + turnIDOfBattleEnd);

        if (currentTurn == turnIDOfBattleEnd)
        {
            //Winning Score Logic
            Destroy(gameObject); //TEMP
        }
    }

    public void addUI(BattleUI _ui)
    {
        ui = _ui;
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

    public int checkPlayerIds(int playerID)
    {
        int howManyIds = 0;

        if(blue.checkID(playerID))
        {
            howManyIds++;
        }

        if (green.checkID(playerID))
        {
            howManyIds++;
        }

        if (yellow.checkID(playerID))
        {
            howManyIds++;
        }

        if (purple.checkID(playerID))
        {
            howManyIds++;
        }

        return howManyIds;
    }

    bool checkPlayerColor(int pID, cardColor color)
    {
        //allows a player to only add to an existing color
        if (color == cardColor.blue)
        {
            if (blue.checkID(pID))
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
            if (green.checkID(pID))
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
            if (yellow.checkID(pID))
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
            if (purple.checkID(pID))
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

            if(checkPlayerColor(playerID, col))
            {
                getColorToModify(col).strength += cd.GetComponent<PirateCard>().strength;
                ui.addToColorInt(col, getColorToModify(col).strength);

                return true;
            }
            else if(!getColorToModify(col).hasID() && checkPlayerIds(playerID) == 0)
            {
                getColorToModify(col).strength += cd.GetComponent<PirateCard>().strength;
                getColorToModify(col).playerID = playerID;
                ui.addToColorInt(col, getColorToModify(col).strength);

                return true;
            }
            else
            {
                return false;
            }
        }
        else if (cd.GetComponent<CaptainCard>())
        {
            col = cd.GetComponent<CaptainCard>().color;

            if (checkPlayerColor(playerID, col))
            {
                winningPlayer = playerID;
                ui.addToColorCaptain(col);

                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    public void cardIntoBattle(Card cd)
    {
        if (cd.GetComponent<PirateCard>())
        {
            cardsInBattle++;

            cd.transform.SetParent(getColorToModify(cd.GetComponent<PirateCard>().color).transform);
            cd.transform.localScale = new Vector3(.9f, .9f, .9f);
            cd.transform.position = cd.transform.transform.parent.position;

            cd.sortingOrder = 5 * cardsInBattle;
            cd.state = cardState.battle;
        }
        else if (cd.GetComponent<CaptainCard>())
        {
            cardsInBattle++;

            if (cd.GetComponent<CaptainCard>().color == cardColor.admiral)
            {
                cd.transform.SetParent(transform);
            }
            else
            {
                cd.transform.SetParent(getColorToModify(cd.GetComponent<CaptainCard>().color).transform);
            }

            cd.transform.localScale = new Vector3(.9f, .9f, .9f);
            cd.transform.position = cd.transform.transform.parent.position;

            cd.sortingOrder = 5 * cardsInBattle;
            cd.state = cardState.battle;
        }
    }

    public void AIClicked()
    {
        Loot.S.SelectedBattle(this);
    }

    public void OnMouseUpAsButton()
    {
        Loot.S.SelectedBattle(this);
    }

    private Vector3 getColorFromPlayerID(int id)
    {
        if(blue.checkID(id))
        {
            return blue.transform.position;
        }
        else if(green.checkID(id))
        {
            return green.transform.position;
        }
        else if(yellow.checkID(id))
        {
            return yellow.transform.position;
        }
        else if(purple.checkID(id))
        {
            return purple.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void hightLightCard(int id)
    {
        playerHighlight.SetActive(true);
        playerHighlight.transform.position = getColorFromPlayerID(id);
    }

    private void OnMouseEnter()
    {
        transform.localScale *= 1.25f;
        ui.transform.localScale *= 1.25f;
    }

    private void OnMouseExit()
    {
        transform.localScale /= 1.25f;
        ui.transform.localScale /= 1.25f;
    }

    private void OnDestroy()
    {
        Loot.S.getPlayerFromId(battleOwner).battles.Remove(this);
        if (ui != null)
        {
            Destroy(ui.gameObject);
        }
    }
}