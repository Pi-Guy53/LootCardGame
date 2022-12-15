using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player
{
    private void Start()
    {
        battles = new List<Battle>();
    }

    public override void StartTurn()
    {
        
    }

    public override Vector3 HomeWaters()
    {
        Vector3 homeWaterPos = homeWaters.transform.position + transform.right * ((battles.Count - 4) * 4);// + transform.up * (-Mathf.Pow((battles.Count * .5f) - 2, 2) + 2);

        for(int i = 0; i < battles.Count; i++)
        {
            if(homeWaterPos == battles[i].transform.position)
            {
                homeWaterPos = homeWaters.transform.position + transform.right * (((battles.Count + 1) - 4) * 4);
            }
        }

        //float c = battles.Count * 4;
        //Vector3 homeWaterPos = transform.up * (-Mathf.Abs(2*battles.Count - 8) -1) + transform.right * (-16 + c);

        return homeWaterPos;
    }

    public override void WaitingForInput()
    {
        //highlight all possible battles to join
    }

    public override void DisplayHand()
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

        Battle[] battles = FindObjectsOfType<Battle>();

        for (int i = 0; i < battles.Length; i++)
        {
            if (battles[i].checkPlayerIds(0) > 0)
            {
                battles[i].hightLightCard(playerID);
            }
        }
    }
}
