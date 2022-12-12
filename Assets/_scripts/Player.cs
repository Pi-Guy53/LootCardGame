using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public Transform handAnchor;
    public int playerID;

    public Transform homeWaters;

    public bool isTurn;
    public List<Battle> battles;

    public int goldCount;

    private void Start()
    {
        battles = new List<Battle>();
    }

    public void AddGold(int amount)
    {
        goldCount += amount;

        print("player[" + playerID + "] gold count = " + goldCount);
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