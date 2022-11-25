using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public ColorToPlayer blue;
    public ColorToPlayer green;
    public ColorToPlayer yellow;
    public ColorToPlayer purple;

    public int turnIDOfBattleEnd;

    private void Start()
    {
        blue.shipColor = cardColor.blue;
        green.shipColor = cardColor.green;
        yellow.shipColor = cardColor.yellow;
        purple.shipColor = cardColor.purple;
    }

    public void newTurn()
    {
        //check battle win conditons
    }
}