using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public Transform handAnchor;
    public int playerID;

    public Transform homeWaters;

    public bool isTurn;
    public List<Battle> battles;

    public int goldCount;

    public Text goldTxt;

    private void Start()
    {
        battles = new List<Battle>();
        AddGold(0);
    }

    public void AddGold(int amount)
    {
        goldCount += amount;

        if (goldTxt != null)
        {
            goldTxt.text = "$" + goldCount;
        } 
    }

    public virtual void StartTurn()
    {
        print("My turn: [" + playerID + "]");
    }

    public void addBattle(Battle newBattle)
    {
        battles.Add(newBattle);
    }

    public virtual Vector3 HomeWaters()
    {
        return Vector3.zero;
    }

    public void drawCard(Card cd)
    {
        hand.Add(cd);
        cd.state = cardState.hand;
        cd.transform.parent = handAnchor;
        cd.transform.rotation = handAnchor.transform.rotation;

        DisplayHand();
    }

    public Card discardCard(Card cd)
    {
        if (hand.Remove(cd))
        {
            print(cd.name);
            DisplayHand();
            return cd;
        }
        else
        {
            return null;
        }
    }

    public virtual void WaitingForInput()
    {
        //highlight all possible battles to join
    }

    public virtual void DisplayHand()
    {

    }

    public bool containsCard(Card cd)
    {
        for (int i = 0; i < hand.Count; i++)
        {
            if (cd == hand[i])
            {
                return true;
            }
        }

        return false;
    }
}