using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardState { battle, deck, hand, discard }

public class Card : MonoBehaviour
{
    private bool isFaceUp;
    public GameObject cardBack;
    public cardState state;

    private Vector3 orPos;
    private bool hover;

    private int sortOrder;

    public bool faceUp
    {
        get
        {
            return isFaceUp;
        }

        set
        {
            isFaceUp = value;
            cardBack.GetComponent<SpriteRenderer>().sortingOrder = sortOrder + 4;
            cardBack.SetActive(!value);
        }
    }

    public int sortingOrder
    {
        get
        {
            return sortOrder;
        }
        set
        {
            sortOrder = value;

            GetComponent<SpriteRenderer>().sortingOrder = value;

            for (int i = 0; i < transform.childCount; i++)
            {
                SpriteRenderer spr = transform.GetChild(i).GetComponent<SpriteRenderer>();

                if(spr.name.Contains("pip"))
                {
                    spr.sortingOrder = sortOrder + 3;
                }
                else if(spr.name.Contains("decor"))
                {
                    spr.sortingOrder = sortOrder + 1;
                }
                else if(spr.name.Contains("trim"))
                {
                    spr.sortingOrder = sortOrder + 2;
                }
                else if(spr.name.Contains("back"))
                {
                    spr.sortingOrder = sortOrder + 4;
                }
            }
        }
    }

    private void OnMouseEnter()
    {
        if (state == cardState.hand)
        {
            orPos = transform.position;
            transform.position += transform.up * .35f;
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
        Loot.S.AICardClicked(this);
    }

    public void OnMouseUpAsButton()
    {
        Loot.S.CardClicked(this, true);
    }

}