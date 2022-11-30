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
    private List<Battle> battles;

    private void Start()
    {
        battles = new List<Battle>();
    }

    public virtual void StartTurn()
    {
        isTurn = true;
    }

    public void addBattle(Battle newBattle)
    {
        battles.Add(newBattle);
    }

    public Vector3 HomeWaters()
    {
        //more leteral spacing, slide whole curve to the left
        Vector3 homeWaterPos = homeWaters.transform.position + transform.right * battles.Count + transform.up * (-Mathf.Pow((battles.Count * .25f) - 2, 2) + 4);

        return homeWaterPos;
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

    public void DisplayHand()
    {
        if (hand.Count < 11)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                hand[i].transform.position = handAnchor.transform.position + transform.right * ((hand.Count / 1) - (i * 2)) + transform.forward * (i * .25f);
                hand[i].faceUp = true;
            }
        }
        else
        {
            for (int i = 0; i < hand.Count; i++)
            {
                hand[i].transform.position = handAnchor.transform.position + transform.right * ((hand.Count / 3) - (i / 1.5f)) + transform.forward * (i * .25f);
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