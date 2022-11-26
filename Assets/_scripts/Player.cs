using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> hand;
    public Transform handAnchor;
    public int playerID;

    private bool isTurn;

    public virtual void StartTurn()
    {
        isTurn = true;
    }

    public void drawCard(Card cd)
    {
        hand.Add(cd);
        cd.state = cardState.hand;
        cd.transform.parent = handAnchor;

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

    public void DisplayHand()
    {
        for(int i = 0; i<hand.Count; i++)
        {
            hand[i].transform.position = handAnchor.transform.position + transform.right * ((hand.Count / 2) - i);
            hand[i].faceUp = true;
        }
    }

}