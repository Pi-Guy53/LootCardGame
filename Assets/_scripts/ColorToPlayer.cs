using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToPlayer : MonoBehaviour
{
    public int playerID;
    public cardColor shipColor;
    public int totalShipStrength;

    public bool checkID(int id)
    {
        return id == playerID;
    }
}