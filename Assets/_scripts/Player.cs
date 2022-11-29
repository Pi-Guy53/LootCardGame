using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public Transform handAnchor;
    public int playerID;

    public Transform homeWaters;

    private bool isTurn;

    public virtual void StartTurn()
    {
        isTurn = true;
    }

    public Vector3 HomeWaters()
    {
        //Temp until I figure out positioning logic
        return homeWaters.position;
    }

    public void drawCard(Card cd)
    {
        hand.Add(cd);
        cd.state = cardState.hand;
        cd.transform.parent = handAnchor;
        cd.transform.rotation = handAnchor.transform.rotation;

        DisplayHand();
    }

    public bool discardCard(Card cd)
    {
        if (hand.Remove(cd))
        {
            DisplayHand();
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void WaitingForInput()
    {
        //highlight all possible battles to join
    }

    public void DisplayHand()
    {
        if (hand.Count < 11)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                hand[i].transform.position = handAnchor.transform.position + transform.right * ((hand.Count / 1) - (i * 2)) + transform.forward * i;
                hand[i].faceUp = true;
            }
        }
        else
        {
            for (int i = 0; i < hand.Count; i++)
            {
                hand[i].transform.position = handAnchor.transform.position + transform.right * ((hand.Count / 3) - (i / 1.5f)) + transform.forward * i;
                hand[i].sortingOrder = (hand.Count - i) * 5;
                hand[i].faceUp = true;
            }
        }
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