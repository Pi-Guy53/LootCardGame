using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardState { battle, deck, hand, discard }
public enum cardColor { green, blue, yellow, purple, admiral}

public class Card : MonoBehaviour
{
    private bool isFaceUp;
    public GameObject cardBack;
    public cardState state;

    public bool faceUp
    {
        get
        {
            return isFaceUp;
        }

        set
        {
            isFaceUp = value;
            cardBack.SetActive(!value);
        }
    }

    public void OnMouseUpAsButton()
    {
        Loot.S.CardClicked(this);
    }

}