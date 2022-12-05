using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public Text goldTxt;

    public Text blueTxt;
    public Text greenTxt;
    public Text yellowTxt;
    public Text purpleTxt;

    public void addToColorInt(cardColor col, int amount)
    {
        switch (col)
        {
            case cardColor.green:
                greenTxt.text = "" + amount;
                break;


            case cardColor.blue:
                blueTxt.text = "" + amount;
                break;

            case cardColor.yellow:
                yellowTxt.text = "" + amount;
                break;


            case cardColor.purple:
                purpleTxt.text = "" + amount;
                break;
        }
    }

    public void addToColorCaptain(cardColor col)
    {
        switch (col)
        {
            case cardColor.green:
                greenTxt.text = "Capt";
                break;


            case cardColor.blue:
                blueTxt.text = "Capt";
                break;

            case cardColor.yellow:
                yellowTxt.text = "Capt";
                break;


            case cardColor.purple:
                purpleTxt.text = "Capt";
                break;

            case cardColor.admiral:
                print("Admiral on deck");
                break;
        }
    }
}