using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Card> hand;

    private bool isTurn;

    public void StartTurn()
    {
        isTurn = true;
    }



}