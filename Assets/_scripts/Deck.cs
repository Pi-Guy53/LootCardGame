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
    public GameObject faceDecorPref;

    public Sprite greenCaptain;
    public Sprite blueCaptain;
    public Sprite yellowCaptain;
    public Sprite purpleCaptain;
    public Sprite admiral;
    public Sprite merchantShip;
    public Sprite pirateShip;

    public Vector2 pipStart;
    public float pipSize;

    private Transform deckAnchor;

    private void Awake()
    {
        deck = new List<Card>();
        deckAnchor = new GameObject("deck").transform;
        deckAnchor.transform.position = Vector3.zero;
    }

    public List<Card> GetDeck()
    {
        return deck;
    }

    public void MoveDeck(Vector3 newPos)
    {
        deckAnchor.transform.position = newPos;
    }

    public void CreateDeck()
    {
        for (int m = 0; m < 25; m++)
        {
            if (m < 5)
            {
                deck.Add(CreateMerchant(2));
            }
            else if (m < 11)
            {
                deck.Add(CreateMerchant(3));
            }
            else if (m < 16)
            {
                deck.Add(CreateMerchant(4));
            }
            else if (m < 21)
            {
                deck.Add(CreateMerchant(5));
            }
            else if (m < 23)
            {
                deck.Add(CreateMerchant(6));
            }
            else if (m < 24)
            {
                deck.Add(CreateMerchant(7));
            }
            else if (m < 25)
            {
                deck.Add(CreateMerchant(8));
            }
        }

        deck.Add(CreateCaptain(cardColor.green));
        deck.Add(CreateCaptain(cardColor.blue));
        deck.Add(CreateCaptain(cardColor.yellow));
        deck.Add(CreateCaptain(cardColor.purple));
        deck.Add(CreateCaptain(cardColor.admiral));

        cardColor col;

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                col = cardColor.green;
            }
            else if (i == 1)
            {
                col = cardColor.blue;
            }
            else if (i == 2)
            {
                col = cardColor.yellow;
            }
            else
            {
                col = cardColor.purple;
            }

            for (int p = 0; p < 12; p++)
            {
                if (p < 2)
                {
                    deck.Add(CreatePirate(1, col));
                }
                else if (p < 6)
                {
                    deck.Add(CreatePirate(2, col));
                }
                else if (p < 10)
                {
                    deck.Add(CreatePirate(3, col));
                }
                else if (p < 12)
                {
                    deck.Add(CreatePirate(4, col));
                }
            }
        }

        ShuffleDeck();
    }

    void ShuffleDeck()
    {
        List<Card> tempDeck = new List<Card>();

        int totalCards = deck.Count;

        for(int i = 0; i < totalCards; i++)
        {
            int rand = Random.Range(0, deck.Count);
            tempDeck.Add(deck[rand]);
            deck.Remove(deck[rand]);
            
        }

        deck = tempDeck;
    }

    GameObject getFaceDecor(Sprite decor, Transform parent)
    {
        GameObject thisObj = Instantiate(faceDecorPref);
        thisObj.GetComponent<SpriteRenderer>().sprite = decor;

        thisObj.transform.position = parent.transform.position;
        thisObj.transform.parent = parent.transform;

        return thisObj;
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

        getFaceDecor(pirateShip, thisPirate.transform);

        thisPirate.cardBack = Instantiate(cardBack);
        thisPirate.cardBack.transform.parent = thisPirate.transform;
        thisPirate.cardBack.transform.position = thisPirate.transform.position;

        thisPirate.strength = str;
        thisPirate.color = color;
        thisPirate.faceUp = false;
        thisPirate.state = cardState.deck;

        thisPirate.name += "-" + color + "-" + str;

        thisPirate.transform.parent = deckAnchor;
        thisPirate.transform.position = deckAnchor.position;

        thisPirate.transform.localScale *= 2;

        return thisPirate;
    }

    CaptainCard CreateCaptain(cardColor color)
    {
        GameObject thisCard = Instantiate(cardTemplate);
        thisCard.AddComponent<CaptainCard>();

        CaptainCard thisCapt = thisCard.GetComponent<CaptainCard>();
        SetTrimColor(thisCard, color);

        switch (color)
        {
            case cardColor.green:
                getFaceDecor(greenCaptain, thisCapt.transform);
                break;

            case cardColor.blue:
                getFaceDecor(blueCaptain, thisCapt.transform);
                break;

            case cardColor.yellow:
                getFaceDecor(yellowCaptain, thisCapt.transform);
                break;

            case cardColor.purple:
                getFaceDecor(purpleCaptain, thisCapt.transform);
                break;

            case cardColor.admiral:
                getFaceDecor(admiral, thisCapt.transform);
                break;

            default:
                getFaceDecor(null, thisCapt.transform);
                break;
        }

        thisCapt.cardBack = Instantiate(cardBack);
        thisCapt.cardBack.transform.parent = thisCapt.transform;
        thisCapt.cardBack.transform.position = thisCapt.transform.position;

        thisCapt.color = color;
        thisCapt.faceUp = false;
        thisCapt.state = cardState.deck;

        thisCapt.name += "-" + color + "-captain";

        thisCapt.transform.parent = deckAnchor;
        thisCapt.transform.position = deckAnchor.position;

        thisCapt.transform.localScale *= 2;

        return thisCapt;
    }

    void SetTrimColor(GameObject cd, cardColor col)
    {
        cd.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = SelectColor.getColor(col);
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

        getFaceDecor(merchantShip, thisMerch.transform);

        thisMerch.cardBack = Instantiate(cardBack);
        thisMerch.cardBack.transform.parent = thisMerch.transform;
        thisMerch.cardBack.transform.position = thisMerch.transform.position;

        thisMerch.goldValue = gv;
        thisMerch.faceUp = false;
        thisMerch.state = cardState.deck;
        thisMerch.name += "-merchant-" + gv;

        thisMerch.transform.parent = deckAnchor;
        thisMerch.transform.position = deckAnchor.position;

        thisMerch.transform.localScale *= 2;

        return thisMerch;
    }
}