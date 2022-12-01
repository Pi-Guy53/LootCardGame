using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardColor { green, blue, yellow, purple, admiral, none }

public class SelectColor
{
    public static Color getColor(cardColor cardCol)
    {
        Color color = Color.white;
        switch (cardCol)
        {
            case cardColor.green:
                color = Color.green;
                break;

            case cardColor.blue:
                color = new Color(0, .5f, 1);//blue
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

        return color;
    }
}