using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGenerator : MonoBehaviour
{
    public int deckNum;
    public int lengthNum;

    public int strength;

    public GameObject foreDeck, foreSubDeck, foreHull;
    public GameObject midDeck, midSubDeck, midHull;
    public GameObject aftDeck, aftSubDeck, aftHull;

    private List<GameObject> hull;

    [ContextMenu("generate ship")]
    public void generateShip()
    {
        hull = new List<GameObject>();

        CalcStrength();
        resetShip();

        GameObject thisforeHull = createPart(foreHull);
        GameObject thisAftHull = createPart(aftHull);

        thisforeHull.transform.position = transform.position + transform.right * ((lengthNum - 1) * 1.5f);

        GameObject thisforDeck = createPart(foreDeck);
        GameObject thisAftDeck = createPart(aftDeck);

        thisforDeck.transform.position = transform.position + transform.right * ((lengthNum - 1) * 1.5f);

        thisforDeck.transform.position += transform.up * (deckNum * .25f);
        thisAftDeck.transform.position = transform.position + transform.up * (deckNum * .25f);

        for (int l = 0; l < lengthNum; l++)
        {
            GameObject thisMidhull = createPart(midHull);
            thisMidhull.transform.position = transform.position + transform.right * (1.5f * l);

            GameObject thisMidDeck = createPart(midDeck);
            thisMidDeck.transform.position = transform.position + transform.right * (1.5f * l) + transform.up * (deckNum * .25f);

            for (int d = 0; d < deckNum; d++)
            {
                GameObject thisMidSubDeck = createPart(midSubDeck);
                thisMidSubDeck.transform.position = transform.position + transform.right * (1.5f * l) + transform.up * ((deckNum - 1) * (d * .25f));

                GameObject thisForeSubDeck = createPart(foreSubDeck);
                thisForeSubDeck.transform.position = transform.position + transform.right * ((lengthNum - 1) * 1.5f) + transform.up * ((deckNum - 1) * (d * .25f));

                GameObject thisAftSubDeck = createPart(aftSubDeck);
                thisAftSubDeck.transform.position = transform.position + transform.up * ((deckNum - 1) * (d * .25f));
            }
        }

        /*
        CombineInstance[] comb = new CombineInstance[hull.Count];

        for(int i = 0; i<hull.Count; i++)
        {
            comb[i].mesh = hull[i].GetComponent<MeshFilter>().mesh;
            comb[i].transform = hull[i].transform.localToWorldMatrix;

            if (hull[i].transform.childCount > 0)
            {
                hull[i].transform.GetChild(0).transform.parent = transform;
            }
            hull[i].SetActive(false);
        }
        GetComponent<MeshFilter>().mesh = new Mesh();
        GetComponent<MeshFilter>().mesh.CombineMeshes(comb);
        */

    }

    void CalcStrength()
    {
        lengthNum = strength;
        deckNum = Mathf.FloorToInt(strength / 2);
    }

    private GameObject createPart(GameObject toCreate)
    {
        GameObject go = Instantiate(toCreate);
        go.transform.parent = transform;
        go.transform.position = transform.position;

        hull.Add(go);

        return go;
    }

    void resetShip()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(0));
        }
    }
}