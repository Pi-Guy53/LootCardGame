using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    private List<Card> deck;

    public GameObject cardTemplate;
    public GameObject goldPip;
    public GameObject skullPip;
    public GameObject cardBack;

    public GameObject greenCaptain;
    public GameObject blueCaptain;
    public GameObject yellowCaptain;
    public GameObject purpleCaptain;
    public GameObject admiral;

    public Vector2 pipStart;
    public float pipSize;

    private void Start()
    {
        deck = new List<Card>();

        CreatePirate(3, cardColor.admiral);
    }

    public void CreateDeck()
    {

    }

    PirateCard CreatePirate(int str, cardColor color)
    {
        GameObject thisCard = Instantiate(cardTemplate);
        thisCard.AddComponent<PirateCard>();

        PirateCard thisPirate = thisCard.GetComponent<PirateCard>();

        SetTrimColor(thisCard, color);

        GameObject pip;

        for (int i = 0; i < str; i++)
        {
            pip = Instantiate(skullPip);
            Vector3 pipPos;

            pipPos.x = (thisPirate.transform.position.x + pipStart.x) + (pipSize * i);
            pipPos.y = thisPirate.transform.position.y + pipStart.y;
            pipPos.z = 0;

            pip.transform.position = pipPos;
            pip.transform.parent = thisPirate.transform;
        }

        for (int i = 0; i < str; i++)
        {
            pip = Instantiate(skullPip);
            Vector3 pipPos;

            pipPos.x = (thisPirate.transform.position.x - pipStart.x) - (pipSize * i);
            pipPos.y = thisPirate.transform.position.y - pipStart.y;
            pipPos.z = 0;

            pip.transform.position = pipPos;
            pip.transform.parent = thisPirate.transform;
        }

        thisPirate.cardBack = Instantiate(cardBack);
        thisPirate.cardBack.transform.parent = thisPirate.transform;
        thisPirate.cardBack.transform.position = thisPirate.transform.position;

        thisPirate.strength = str;
        thisPirate.faceUp = false;
        thisPirate.state = cardState.deck;

        return thisPirate;
    }

    void SetTrimColor(GameObject cd, cardColor col)
    {
        Color color = Color.white;
        switch (col)
        {
            case cardColor.green:
                color = Color.green;
                break;

            case cardColor.blue:
                color = Color.blue;
                break;

            case cardColor.yellow:
                color = Color.yellow;
                break;

            case cardColor.purple:
                color = new Color(.5f, 0, 1f); //purple
                break;

            case cardColor.admiral:
                color = new Color(.75f, 0, 0); //red
                break;
        }

        cd.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = color;
    }

    MerchantCard CreateMerchant(int gv)
    {
        GameObject thisCard = Instantiate(cardTemplate);
        thisCard.AddComponent<MerchantCard>();

        MerchantCard thisMerch = thisCard.GetComponent<MerchantCard>();

        GameObject pip;

        for (int i = 0; i < gv; i++)
        {
            pip = Instantiate(goldPip);
            Vector3 pipPos;

            if (i % 2 == 0)
            {
                pipPos.x = (thisMerch.transform.position.x + pipStart.x);
                pipPos.y = (thisMerch.transform.position.y + pipStart.y) - ((pipSize / 2) * i);
            }
            else
            {
                pipPos.x = (thisMerch.transform.position.x + pipStart.x) + pipSize;
                pipPos.y = (thisMerch.transform.position.y + pipStart.y) - ((pipSize / 2) * (i -1));
            }

            pipPos.z = 0;

            pip.transform.position = pipPos;
            pip.transform.parent = thisMerch.transform;
        }

        for (int i = 0; i < gv; i++)
        {
            pip = Instantiate(goldPip);
            Vector3 pipPos;

            if (i % 2 == 0)
            {
                pipPos.x = (thisMerch.transform.position.x + -pipStart.x);
                pipPos.y = (thisMerch.transform.position.y + -pipStart.y) + ((pipSize / 2) * i);
            }
            else
            {
                pipPos.x = (thisMerch.transform.position.x + -pipStart.x) - pipSize;
                pipPos.y = (thisMerch.transform.position.y + -pipStart.y) + ((pipSize / 2) * (i - 1));
            }

            pipPos.z = 0;

            pip.transform.position = pipPos;
            pip.transform.parent = thisMerch.transform;
        }

        thisMerch.cardBack = Instantiate(cardBack);
        thisMerch.cardBack.transform.parent = thisMerch.transform;
        thisMerch.cardBack.transform.position = thisMerch.transform.position;

        thisMerch.goldValue = gv;
        thisMerch.faceUp = false;
        thisMerch.state = cardState.deck;

        return thisMerch;
    }

    public void ShuffleDeck()
    {

    }
}