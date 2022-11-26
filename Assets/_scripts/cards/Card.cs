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

    private Vector3 orPos;
    private bool hover;

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

    private void OnMouseEnter()
    {
        if (state == cardState.hand)
        {
            orPos = transform.position;
            transform.position += transform.up * .25f;
            hover = true;
        }
    }

    private void OnMouseExit()
    {
        if (hover)
        {
            transform.position = orPos;
            hover = false;
        }
    }

    public void AIClicked()
    {
        Loot.S.CardClicked(this, false);
    }

    public void OnMouseUpAsButton()
    {
        Loot.S.CardClicked(this, true);
    }

}