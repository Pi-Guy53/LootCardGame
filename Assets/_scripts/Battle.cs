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
        if (currentTurn == turnIDOfBattleEnd)
        {
            print(turnIDOfBattleEnd + " : " + currentTurn);

            int i = winningPlayerColor();

            if (winningPlayer != -1)
            {
                i = winningPlayer;
            }

            if (TiedStrength() && winningPlayer == -1)
            {
                print("tied strengths");
            }
            else
            {
                if (i == -1)
                {
                    print("owner won");
                    Loot.S.AwardGoldToID(battleOwner, goldValue, transform.position);
                }
                else
                {
                    print("highest score won");
                    Loot.S.AwardGoldToID(i, goldValue, transform.position);
                }

                Destroy(gameObject);
            }
        }
    }

    public void addUI(BattleUI _ui)
    {
        ui = _ui;
    }

    bool TiedStrength()
    {
        List<ColorToPlayer> activeColors = new List<ColorToPlayer>();

        if (blue.strength == 0)
        {
            //exclude blue
        }
        else
        {
            activeColors.Add(blue);
        }

        if (green.strength == 0)
        {
            //exclude green
        }
        else
        {
            activeColors.Add(green);
        }

        if (yellow.strength == 0)
        {
            //exclude yellow
        }
        else
        {
            activeColors.Add(yellow);
        }

        if (purple.strength == 0)
        {
            //exclude purple
        }
        else
        {
            activeColors.Add(purple);
        }

        activeColors.Sort(ColorToPlayer.CompareTo);

        if (activeColors.Count > 1)
        {
            if (activeColors[activeColors.Count - 1].strength == activeColors[activeColors.Count - 2].strength)
            {
                return true;
            }
        }

        return false;
    }

    public int winningPlayerColor()
    {
        int i = 0;
        int id = -1;

        if(blue.strength > i)
        {
            i = blue.strength;
            id = blue.playerID;
        }

        if(green.strength > i)
        {
            i = green.strength;
            id = green.playerID;
        }

        if(yellow.strength > i)
        {
            i = yellow.strength;
            id = yellow.playerID;
        }

        if(purple.strength > i)
        {
            i = purple.strength;
            id = purple.playerID;
        }

        return id;
    }

    public bool winningOwner(int id)
    {
        if(winningPlayerColor() == -1 && id == battleOwner)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public ColorToPlayer getColorToModify(cardColor col)
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

    public bool checkPlayerColor(int pID, cardColor color)
    {
        //allows a player to only add to an existing color
        if (color == cardColor.blue)
        {
            if (blue.checkID(pID))
            {
                return true;
            }
        }

        if (color == cardColor.green)
        {
            if (green.checkID(pID))
            {
                return true;
            }
        }

        if (color == cardColor.yellow)
        {
            if (yellow.checkID(pID))
            {
                return true;
            }
        }

        if (color == cardColor.purple)
        {
            if (purple.checkID(pID))
            {
                return true;
            }
        }

        if (color == cardColor.admiral)
        {
            if (battleOwner == pID)
            {
                winningPlayer = pID;
                return true;
            }

        }

        return false;
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

                turnIDOfBattleEnd = playerID;

                return true;
            }
            else if(!getColorToModify(col).hasID() && checkPlayerIds(playerID) == 0)
            {
                getColorToModify(col).strength += cd.GetComponent<PirateCard>().strength;
                getColorToModify(col).playerID = playerID;
                ui.addToColorInt(col, getColorToModify(col).strength);

                turnIDOfBattleEnd = playerID;

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

                turnIDOfBattleEnd = playerID;

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

            cd.destination = cd.transform.parent;
            //cd.transform.position = cd.transform.transform.parent.position;

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

            cd.destination = cd.transform.parent;
            //cd.transform.position = cd.transform.transform.parent.position;

            cd.sortingOrder = 5 * cardsInBattle;
            cd.state = cardState.battle;
        }
    }

    public bool isColorFree(cardColor col, int id)
    {
        if(getColorToModify(col).checkID(id) || !getColorToModify(col).hasID())
        {
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